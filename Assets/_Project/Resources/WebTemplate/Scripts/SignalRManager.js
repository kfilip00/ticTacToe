let connection = null;
let unityObjectName = "";

function StartConnection(jsonData) 
{
    const data = JSON.parse(jsonData);
    const url = data.Url;
    const receiverObjectName = data.ReceiverObjectName;
    
    unityObjectName = receiverObjectName;
    if (connection) 
    {
        TellUnityAboutConnectionResult(CreateConnectionResponse(2, "Already connected"));
        return;
    }
    
    connection = new signalR.HubConnectionBuilder()
        .withUrl(url)
        .withAutomaticReconnect()
        .build();

    connection.on("ReceiveMessageFromServer", function (functionName,jsonData) 
    {
        unityInstance.SendMessage(unityObjectName, 'ReceiveMessageFromServerJS', functionName,jsonData);
    });

    connection.start().then(function () 
    {
        TellUnityAboutConnectionResult(CreateConnectionResponse(1, "Successfully connected"));
    }).catch(function (err) 
    {
        console.log(err);
        TellUnityAboutConnectionResult(CreateConnectionResponse(0,"Failed to connect, check browser console"))
    });
}

function CreateConnectionResponse(status, message) {
    return JSON.stringify({
        ConnectionStatus: status,
        Message: message
    });
}

function TellUnityAboutConnectionResult(jsonData)
{
    unityInstance.SendMessage(unityObjectName, 'ReceiveConnectionStatusFromJS', jsonData);
}

function TalkToServer(jsonString) 
{
    const data = JSON.parse(jsonString);
    const functionName = data.FunctionName;
    const jsonData = data.JsonData;
    connection.send(functionName, jsonData);
}