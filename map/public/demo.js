var userLocaitons = {};
var map;

function addUserToMap(user) {

  var icon = user.avtUrl != '' ? new H.map.Icon(user.avtUrl, { size: { w: 56, h: 56 } }) : undefined;

  let marker = icon != undefined ? new H.map.Marker({ lat: user.lat, lng: user.lng },
    { icon: icon }) : new H.map.Marker({ lat: user.lat, lng: user.lng });

  marker.draggable = true;

  let userObj = {
    marker: marker,
    fullName: user.fullName,
    phoneNumber: user.phoneNumber,
  };

  userLocaitons[user.id] = userObj;

  console.log(userLocaitons);

  map.addObject(userLocaitons[user.id].marker);
}

function updateUserLocation(userId, lat, lng) {

  if (userLocaitons[userId] == undefined)
    return;

  userLocaitons[userId].marker.setGeometry({ lat: lat, lng: lng });

}

/**
 * Adds markers to the map highlighting the locations of the captials of
 * France, Italy, Germany, Spain and the United Kingdom.
 *
 * @param  {H.Map} map      A HERE Map instance within the application
 */
function addMarkersToMap(map) {
  var parisMarker = new H.map.Marker({ lat: 10.827200, lng: 106.722928 });
  map.addObject(parisMarker);

}


function initMap(data) {
  /**
   * Boilerplate map initialization code starts below:
   */
  // Step 1: initialize communication with the platform
  // In your own code, replace variable window.apikey with your own apikey
  var platform = new H.service.Platform({
    apikey: data.apiKey
  });
  var defaultLayers = platform.createDefaultLayers();

  var mapContainer = document.getElementById('map');

  let centerPoint = { lat: data.lat, lng: data.lng };
  // let centerPoint = { lat: 10.827200, lng: 106.722928 };

  // Step 2: initialize a map
  map = new H.Map(mapContainer, defaultLayers.vector.normal.map, {
    // initial center and zoom level of the map
    zoom: 16,
    // Champs-Elysees
    center: centerPoint,
    pixelRatio: window.devicePixelRatio || 1
  });

  // add a resize listener to make sure that the map occupies the whole container
  window.addEventListener('resize', () => map.getViewPort().resize());

  // Step 3: make the map interactive
  // MapEvents enables the event system
  // Behavior implements default interactions for pan/zoom (also on mobile touch environments)
  var behavior = new H.mapevents.Behavior(new H.mapevents.MapEvents(map));

  // Step 4: Create the default UI
  var ui = H.ui.UI.createDefault(map, defaultLayers, 'en-US');

  map.addEventListener('dragstart', function (ev) {
    var target = ev.target,
      pointer = ev.currentPointer;
    if (target instanceof H.map.Marker) {
      var targetPosition = map.geoToScreen(target.getGeometry());
      target['offset'] = new H.math.Point(pointer.viewportX - targetPosition.x, pointer.viewportY - targetPosition.y);
      behavior.disable();
    }
  }, false);


  // re-enable the default draggability of the underlying map
  // when dragging has completed
  map.addEventListener('dragend', function (ev) {
    var target = ev.target;
    if (target instanceof H.map.Marker) {
      behavior.enable();
      console.log(ev.target.b);
    }
  }, false);

  // Listen to the drag event and move the position of the marker
  // as necessary
  map.addEventListener('drag', function (ev) {
    var target = ev.target,
      pointer = ev.currentPointer;
    if (target instanceof H.map.Marker) {
      target.setGeometry(map.screenToGeo(pointer.viewportX - target['offset'].x, pointer.viewportY - target['offset'].y));
    }
  }, false);

}