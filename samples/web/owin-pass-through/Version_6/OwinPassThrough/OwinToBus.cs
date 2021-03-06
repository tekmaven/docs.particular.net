﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

#region OwinToBus

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

public class OwinToBus
{
    IBusContext bus;
    Newtonsoft.Json.JsonSerializer serializer;

    public OwinToBus(IBusContext bus)
    {
        this.bus = bus;
        serializer = new Newtonsoft.Json.JsonSerializer();
    }

    public Func<AppFunc, AppFunc> Middleware()
    {
        return _ => Invoke;
    }

    async Task Invoke(IDictionary<string, object> environment)
    {
        string messageBody = await GetMessageBody(environment).ConfigureAwait(false);
        IDictionary<string, string[]> requestHeaders = (IDictionary<string, string[]>) environment["owin.RequestHeaders"];
        string typeName = requestHeaders["MessageType"].Single();
        Type objectType = Type.GetType(typeName);
        object deserialize = Deserialize(messageBody, objectType);
        await bus.SendLocal(deserialize).ConfigureAwait(false);
    }

    object Deserialize(string messageBody, Type objectType)
    {
        using (StringReader textReader = new StringReader(messageBody))
        {
            return serializer.Deserialize(textReader, objectType);
        }
    }

    static async Task<string> GetMessageBody(IDictionary<string, object> environment)
    {
        using (Stream requestStream = (Stream) environment["owin.RequestBody"])
        using (StreamReader streamReader = new StreamReader(requestStream))
        {
            return await streamReader.ReadToEndAsync().ConfigureAwait(false);
        }
    }
}

#endregion
