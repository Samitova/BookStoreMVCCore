﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace BookStore.Web
{
    public static class SessionsExtensions
    {  
        public static void SetObject<T>(this ISession session, string key, T value) 
        {
            if(value == null)
                return;
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static bool TryGetObject<T>(this ISession session, string key, out T value)
        {
            if (session.TryGetValue(key, out _))
            {
                value = JsonConvert.DeserializeObject<T>(session.GetString(key));
                return true;
            }
            value = default(T);
            return false;
        }
    }
}
