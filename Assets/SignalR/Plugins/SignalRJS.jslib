mergeInto(LibraryManager.library,
{
    StartConnectionJS: function(jsonData)
    {
        StartConnection(UTF8ToString(jsonData));
    },   
     
    TalkToServerJS: function(jsonData)
    {
        TalkToServer(UTF8ToString(jsonData));
    }
});