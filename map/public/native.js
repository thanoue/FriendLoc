
var commands = {
    addNewUser: 'addNewUser',
    addUserList: 'addUserList',
    updaterUserLocation: 'updaterUserLocation',
    removeUser: 'removeUser',
    initMap: 'initMap',
    documentIsReady: 'documentIsReady',
    addCoordinate: 'addCoordinate',
    locationUpdated: 'locationUpdated',
    locationChaningRequest: 'locationChaningRequest'
}

function docReady(fn) {
    // see if DOM is already available
    if (document.readyState === "complete" || document.readyState === "interactive") {
        // call on next available tick
        setTimeout(fn, 1);
    } else {
        document.addEventListener("DOMContentLoaded", fn);
    }
}

docReady(function () {

    doNativeMethod(commands.documentIsReady, '');

});

function locactionChanged(latitude, longitude) {

    let obj = {
        latitude: latitude,
        longitude: longitude
    }

    doNativeMethod(commands.locationUpdated, JSON.stringify(obj));

}

function doNativeMethod(commands, data) {

    var res = {
        command: commands,
        data: data
    }

    if (typeof Android !== "undefined" && Android !== null) {

        Android.nativeInvoke(JSON.stringify(res));

    } else {

        webkit.messageHandlers.callback.postMessage(JSON.stringify(res));
    }

}

function getDataFromNativeApp(command, data) {

    let obj = JSON.parse(data);

    switch (command) {
        case commands.addNewUser:
            addNewUser(obj);
            break;
        case commands.updaterUserLocation:
            userLocationUpdate(obj);
            break;
        case commands.addUserList:
            addUsers(obj);
            break;
        case commands.removeUser:
            removeUser(obj);
            break;
        case commands.initMap:
            initMap(obj);
            break;
        case commands.addCoordinate:
            addCoordinate(obj);
            break;
        case commands.locationChaningRequest:
            updateNewCoordinate(obj);
            break;
    }

}

function userLocationUpdate(data) {
    updateUserLocation(data.userId, data.lat, data.lng)
}

function removeUser(data) {

}

function addNewUser(data) {
    addUserToMap(data);
}

function addUsers(data) {
    data.forEach(user => {
        addUserToMap(user);
    });
}
