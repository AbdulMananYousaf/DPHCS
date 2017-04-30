$.connection.hub.start()
    .done(function () {
        console.log("SignalR Working");
    })
    .fail(function () {
        alert("Erroe!!SignalR Not Working");
    });

$.connection.ChatHub.client.announce = function () { alert(message) }