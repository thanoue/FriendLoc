
var commands = {
    addNewUser: 'addNewUser',
    addUserList: 'addUserList',
    updaterUserLocation: 'updaterUserLocation',
    removeUser: 'removeUser',
    initMap: 'initMap',
    documentIsReady: 'documentIsReady'
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

    // // DOM is loaded and ready for manipulation here
    // getDataFromNativeApp(commands.initMap, JSON.stringify({
    //     lat: 10.877500,
    //     lng: 106.722928,
    //     apiKey: 'FJqTTDyUrZA2WiGRgXWfkWEfdK98VSCgHpGpn1bkvMM'
    // }));

    // setTimeout(() => {

    //     let users = [{
    //         avtUrl: '',
    //         lat: 10.879700,
    //         lng: 106.722928,
    //         id: '192jgsdwudfsf',
    //         fullName: 'Traafn kha',
    //         phoneNumber: '09018616'
    //     }, {
    //         avtUrl: '',
    //         lat: 10.878400,
    //         lng: 106.722928,
    //         id: '192fjudfsf',
    //         fullName: 'Traafn kha',
    //         phoneNumber: '09018616'
    //     }, {
    //         avtUrl: 'https://cdn3.iconfinder.com/data/icons/tourism/eiffel200.png',
    //         lat: 10.877500,
    //         lng: 106.722928,
    //         id: '192jsudfsf',
    //         fullName: 'Traafn kha',
    //         phoneNumber: '09018616'
    //     }];

    //     getDataFromNativeApp(commands.addUserList, JSON.stringify(users));

    // }, 3000);

});

function doNativeMethod(commands, data) {

    if (typeof Android !== "undefined" && Android !== null) {

        Android.nativeInvoke(commands, data);

    } else {

        var res = {
            command: commands,
            data: data
        }

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
