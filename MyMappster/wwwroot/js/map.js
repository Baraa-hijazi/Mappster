const mapOptions = {
    mapTypeId: 'roadmap',
    mapTypeControl: false,
    scrollwheel: true,
    streetViewControl: false,
    fullscreenControl: false,
    zoom: 12,
    center: {lat: 25.800735, lng: 55.976242},
    styles: [
        {featureType: 'poi', stylers: [{visibility: 'off'}]},
        {featureType: 'poi.business', stylers: [{visibility: 'off'}]},
        {featureType: 'road', elementType: 'labels', stylers: [{visibility: 'off'}]},
        {featureType: 'transit', elementType: 'labels.icon', stylers: [{visibility: 'off'}]},
        {featureType: 'administrative.locality', elementType: 'labels', stylers: [{visibility: 'off'}]},
        {featureType: 'administrative.neighborhood', elementType: 'labels', stylers: [{visibility: 'off'}]}
    ],
};

let map;
const svgMarker = {
    path: "M-1.547 12l6.563-6.609-1.406-1.406-5.156 5.203-2.063-2.109-1.406 1.406zM0 0q2.906 0 4.945 2.039t2.039 4.945q0 1.453-0.727 3.328t-1.758 3.516-2.039 3.070-1.711 2.273l-0.75 0.797q-0.281-0.328-0.75-0.867t-1.688-2.156-2.133-3.141-1.664-3.445-0.75-3.375q0-2.906 2.039-4.945t4.945-2.039z",
    fillColor: "blue",
    fillOpacity: 0.6,
    strokeWeight: 0,
    rotation: 0,
    scale: 2,
};

let markers = []; // Track current markers
let polygons = []; // Track current polygons
let polyLines = []; // Track current polyLines
let currentInfoWindow = null; // This will hold the currently open InfoWindow

async function initMap() {
    map = new google.maps.Map(document.getElementById("map"), mapOptions);

    // Add click listener on the map
    map.addListener('click', async function (mapsMouseEvent) {
        // Get the click location
        const clickLat = mapsMouseEvent.latLng.lat();
        const clickLng = mapsMouseEvent.latLng.lng();

        // Clear existing area polygons, postal code polygons, and street polyLines
        clearMarkers();
        clearAreaPolygons();
        clearPolyLines();

        // Fetch and display point markers
        const markerData = await getMarkerData(clickLat, clickLng);
        drawMarker(markerData);

        // Fetch and display area polygon
        const areaData = await getAreaData(clickLat, clickLng);
        drawAreaPolygon(areaData);

        // Fetch and display postal code polygon
        const postalCodeData = await getPostalCodeData(clickLat, clickLng);
        drawPostalCodePolygon(postalCodeData);

        // Fetch and display street polyline
        const streetData = await getStreetData(clickLat, clickLng);
        drawPolyline(streetData);
    });
}

async function getMarkerData(lat, lng) {
    const response = await fetch(`/api/points?lat=${lat}&lng=${lng}`);
    return await response.json();
}

function drawMarker(data) {
    if (!data) {
        return;
    }

    data.forEach(point => {
        // Construct content for the info window
        const contentString = `
            <div>
                <h3>Location Details</h3>
                <p><b>Community (EN):</b> ${point.communityNameEn || 'N/A'}</p>
                <p><b>Community (AR):</b> ${point.communityNameAr || 'N/A'}</p>
                <p><b>Area Name (EN):</b> ${point.areaNameEn || 'N/A'}</p>
                <p><b>Area Name (AR):</b> ${point.areaNameAr || 'N/A'}</p>
                <p><b>Region:</b> ${point.region || 'N/A'}</p>
                <p><b>Emirate:</b> ${point.emirate || 'N/A'}</p>
                <p><b>PostCode:</b> ${point.postCode || 'N/A'}</p>
                <p><b>Country:</b> ${point.countryName || 'N/A'}</p>
                <p><b>Full Road Name (EN):</b> ${point.fullRoadNameEn || 'N/A'}</p>
                <p><b>Full Road Name (AR):</b> ${point.fullRoadNameAr || 'N/A'}</p>
                <p><b>Road Type:</b> ${point.roadType || 'N/A'}</p>
                <p><b>Side:</b> ${point.side || 'N/A'}</p>
            </div>`;

        // Create a marker for this point
        const marker = new google.maps.Marker({
            position: {lat: point.latitude, lng: point.longitude},
            map: map,
            icon: svgMarker
        });

        // Create an info window and attach it to the marker
        const infowindow = new google.maps.InfoWindow({
            content: contentString
        });

        marker.addListener('click', function () {
            // Close the current info window if it exists and is open
            if (currentInfoWindow) {
                currentInfoWindow.close();
            }
            // Open the new info window and update the reference
            infowindow.open(map, marker);
            currentInfoWindow = infowindow;
        });

        markers.push(marker);

        // Tooltip logic
        const mapTooltip = document.getElementById('mapTooltip');

        google.maps.event.addListener(marker, 'mouseover', function(event) {
            mapTooltip.style.display = 'block';
            mapTooltip.style.left = event.domEvent.clientX + 'px';
            mapTooltip.style.top = event.domEvent.clientY + 'px';
            mapTooltip.innerHTML = 'Click for more info';
        });

        google.maps.event.addListener(marker, 'mouseout', function() {
            mapTooltip.style.display = 'none';
        });
    });
}

function clearMarkers() {
    markers.forEach(marker => marker.setMap(null));
    markers = [];
}

async function getAreaData(lat, lng) {
    const response = await fetch(`/api/areas?lat=${lat}&lng=${lng}`);
    return await response.json();
}

function drawAreaPolygon(data) {
    if (!data.polygonData) {
        return;
    }
    const coordinates = data.polygonData.coordinates[0].map(coord => ({lat: coord[1], lng: coord[0]}));
    const polygon = new google.maps.Polygon({
        paths: coordinates,
        strokeColor: '#FFCCCC',
        clickable: false,
        strokeOpacity: 0,
        fillColor: '#FFCCCC',
        fillOpacity: 0.35,
        map: map
    });

    google.maps.event.addListener(map, "click", () => {
        clearMarkers();
        clearAreaPolygons();
        clearPolyLines();
    });

    polygon.setMap(map);
    polygons.push(polygon);

    // Draw dashed outline using polyline
    const polyLine = new google.maps.Polyline({
        path: coordinates,
        strokeColor: '#505050',
        strokeOpacity: 0, // Invisible but clickable line
        icons: [{
            icon: {
                path: 'M 0,-1 0,1',
                strokeOpacity: 0.7,
                scale: `2`,
            },
            offset: '0',
            repeat: '8px'
        }],
        map: map,
    });

    polyLine.setMap(map);
    polyLines.push(polyLine);

    // Calculate center of the polygon
    const center = calculatePolygonCenter(polygon);
    const marker = new google.maps.Marker({
        position: center,
        map: map,
        icon: {
            path: google.maps.SymbolPath.CIRCLE,
            fillOpacity: 0,
            strokeOpacity: 0,
            scale: 0 // Invisible marker
        },
        label: {
            text: data.areaName + " - " + data.nameArabi,
            fontWeight: "bold",
            fontSize: "14px", // Make visible
            color: "#343434" // Ensure label is visible
        }
    });

    markers.push(marker);
}

function clearAreaPolygons() {
    polygons.forEach(polygon => polygon.setMap(null));
    polygons = [];
}

function clearPolyLines() {
    polyLines.forEach(polyLine => polyLine.setMap(null));
    polyLines = [];
}

function drawPostalCodePolygon(data) {
    if (!data.polygonData) {
        return;
    }

    const coordinates = data.polygonData.coordinates[0].map(coord => ({lat: coord[1], lng: coord[0]}));
    const polygon = new google.maps.Polygon({
        paths: coordinates,
        clickable: false,
        strokeColor: 'blue',
        strokeOpacity: 0,
        fillColor: 'blue',
        fillOpacity: 0.35,
        map: map
    });

    google.maps.event.addListener(map, "click", () => {
        clearMarkers();
        clearAreaPolygons();
        clearPolyLines();
    });

    polygons.push(polygon);

    // Draw dashed outline using polyline
    const polyLine = new google.maps.Polyline({
        path: coordinates,
        clickable: false,
        strokeColor: '#505050',
        strokeOpacity: 0, // Invisible but clickable line
        icons: [{
            icon: {
                path: 'M 0,-1 0,1',
                strokeOpacity: 0.7,
                scale: `2`,
            },
            offset: '0',
            repeat: '8px'
        }],
        map: map,
    });

    google.maps.event.addListener(map, "click", () => {
        clearMarkers();
        clearAreaPolygons();
        clearPolyLines();
    });

    polyLine.setMap(map);
    polyLines.push(polyLine);

    // Calculate center of the polygon
    const center = calculatePolygonCenter(polygon);
    const marker = new google.maps.Marker({
        position: center,
        map: map,
        icon: {
            path: google.maps.SymbolPath.CIRCLE,
            fillOpacity: 0,
            strokeOpacity: 0,
            scale: 0 // Invisible marker
        },
        label: {
            text: data.emPostCode,
            fontWeight: "bold",
            fontSize: "14px",
            color: "#343434"
        }
    });

    markers.push(marker);
}

function calculatePolygonCenter(polygon) {
    const bounds = new google.maps.LatLngBounds();
    polygon.getPath().forEach(function (element) {
        bounds.extend(element)
    });
    return bounds.getCenter();
}

async function getPostalCodeData(lat, lng) {
    const response = await fetch(`/api/postalcodes?lat=${lat}&lng=${lng}`);
    return await response.json();
}

async function getStreetData(lat, lng) {
    const response = await fetch(`/api/streets?lat=${lat}&lng=${lng}`);
    if (!response.ok) {
        console.info("No street data found:");
        return {streetData: {coordinates: []}};
    }

    return await response.json();
}

function drawPolyline(data) {
    if (!data.streetData) {
        return;
    }

    const lineCoordinates = data.streetData.coordinates.map(c => ({
        lat: c[1],
        lng: c[0]
    }));

    const streetLine = new google.maps.Polyline({
        path: lineCoordinates,
        strokeColor: '#27278a',
        strokeOpacity: 0.9,
        strokeWeight: 5,
    });

    streetLine.setMap(map);
    polyLines.push(streetLine);

    const contentString = `
            <div>
            <h3>Street Name</h3>
            <p><b>Street Name (EN):</b> ${data.Full_St_Name_En || 'N/A'}</p>
            <p><b>Street Name (AR):</b> ${data.Full_St_Name_Ar || 'N/A'}</p>
            </div>`;

    // Create an info window and attach it to the marker
    const infowindow = new google.maps.InfoWindow({
        content: contentString
    });

    // Add a click listener to the polyline to show the info window
    streetLine.addListener('click', (event) => {
        // Use the position of the click to open the info window
        infowindow.setPosition(event.latLng);

        // Close the current info window if it exists and is open
        if (currentInfoWindow) {
            currentInfoWindow.close();
        }

        // Open the new info window and update the reference
        infowindow.open(map, streetLine);
        currentInfoWindow = infowindow;
    });

    // Tooltip logic
    const mapTooltip = document.getElementById('mapTooltip');

    google.maps.event.addListener(streetLine, 'mouseover', function(event) {
        mapTooltip.style.display = 'block';
        mapTooltip.style.left = event.domEvent.clientX + 'px';
        mapTooltip.style.top = event.domEvent.clientY + 'px';
        mapTooltip.innerHTML = 'Click for more info';
    });

    google.maps.event.addListener(streetLine, 'mouseout', function() {
        mapTooltip.style.display = 'none';
    });
}