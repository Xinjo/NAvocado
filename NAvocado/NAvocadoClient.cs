using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using NAvocado.Exceptions;
using NAvocado.Extensions;

/*
Recipe — The world's best guacamole recipe.

1. Two near-ripe avocados, lightly smooshed
2. Juice from one lime
3. A dash of kosher salt
4. Ground cumin & cayenne pepper to taste (optional)
5. Freshly ground pepper to taste
6. Nothing else

We will guac your world.
*/

namespace NAvocado
{
    // TODO#000: Implement all the functions that the API provides, link: https://avocado.io/guacamole/avocado-api
    // TODO#001: List Items (Create/Edit/Delete)
    // TODO#002: Calender Models (Event/Reminder)
    // TODO#003: Calender Methods (Events Show/Create/Edit/Delete) (Reminders Create/Edit/Delete)
    // TODO#004: Media (Upload/Listing/Delete)
    // TODO#099: Password is no longer 'secure' after calling ConvertToUnsecureString() (seems obvious), it is visible in memory and therefor a better solution will be required
    // TODO#100: Find a more fancy and easier to read solution to get the cookie value
    public class NAvocadoClient
    {
        /// <summary>
        ///     UserAgent that is send with the request, identifying that an application used this library.
        /// </summary>
        public const string UserAgent = "NAvocado v0.0.1";

        /// <summary>
        ///     The Email that the user/client uses to autheticate itself with the Avocado API.
        /// </summary>
        public string Email { get; private set; } = string.Empty;

        /// <summary>
        ///     Url: https://avocado.io/api/
        /// </summary>
        public const string ApiUrlBase = "https://avocado.io/api/";

        /// <summary>
        ///     Url: https://avocado.io/api/authentication/login
        /// </summary>
        public const string ApiUrlLogin = ApiUrlBase + "authentication/login";

        /// <summary>
        ///     Url: https://avocado.io/api/authentication/logout
        /// </summary>
        public const string ApiUrlLogout = ApiUrlBase + "authentication/logout";

        /// <summary>
        ///     Url: https://avocado.io/api/user/
        /// </summary>
        public const string ApiUrlUser = ApiUrlBase + "user/";

        /// <summary>
        ///     Url: https://avocado.io/api/couple/
        /// </summary>
        public const string ApiUrlCouple = ApiUrlBase + "couple/";

        /// <summary>
        ///     Url: https://avocado.io/api/activities/
        /// </summary>
        public const string ApiUrlActivities = ApiUrlBase + "activities/";

        /// <summary>
        ///     Url: https://avocado.io/api/lists/
        /// </summary>
        public const string ApiUrlLists = ApiUrlBase + "lists/";

        /// <summary>
        ///     Url: https://avocado.io/api/conversation/
        /// </summary>
        public const string ApiUrlConversation = ApiUrlBase + "conversation/";

        /// <summary>
        ///     Url: https://avocado.io/api/conversation/kiss/
        /// </summary>
        public const string ApiUrlConversationKiss = ApiUrlConversation + "kiss/";

        /// <summary>
        ///     Url: https://avocado.io/api/conversation/hug/
        /// </summary>
        public const string ApiUrlConversationHug = ApiUrlConversation + "hug/";

        /// <summary>
        ///     Url: https://avocado.io/api/calender/
        /// </summary>
        public const string ApiUrlCalendar = ApiUrlBase + "calendar/";

        /// <summary>
        ///     Url: https://avocado.io/api/media/
        /// </summary>
        public const string ApiUrlMedia = ApiUrlBase + "media/";

        /// <summary>
        ///     The Developer ID provided to you by Avocado.
        /// </summary>
        private readonly string _devId;

        /// <summary>
        ///     The Developer Key provided to you by Avocado.
        /// </summary>
        private readonly string _devKey;

        /// <summary>
        ///     Underlying <see cref="HttpClient"/> that will handle all requests
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        ///     Cookie Name
        /// </summary>
        private string _cookieName = "user_email";

        /// <summary>
        ///     The Cookie value returned by Avocado after a successful request.
        /// </summary>
        private string _cookieValue;

        /// <summary>
        ///     A SHA256 hashed string that indicates that the intial authetication with Avocado was successful, every request with
        ///     the Avocado API requires this value to be present during the request.
        /// </summary>
        private string _signature;

        /// <summary>
        ///     Create a new instance of the <see cref="NAvocadoClient" /> class, this will handle all communication between the
        ///     application and Avocado API endpoint.
        /// </summary>
        /// <param name="devId">Avocado Developer ID</param>
        /// <param name="devKey">Avocado Developer Key</param>
        public NAvocadoClient(string devId, string devKey)
        {
            _devId = devId;
            _devKey = devKey;

            _httpClient = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
        }

        /// <summary>
        ///     Autheticate the <see cref="NAvocadoClient" /> with the Avocado API using the email and password provided by the
        ///     user.
        /// </summary>
        /// <param name="email">Email provided</param>
        /// <param name="password"></param>
        /// <returns>True if authentication was successful; otherwise false</returns>
        /// <exception cref="AuthenticationFailedException"></exception>
        public async Task<bool> Login(string email, SecureString password)
        {
            // TODO#002: Password is no longer 'secure' after calling ConvertToUnsecureString() (seems obvious), it is visible in memory and therefor a better solution will be required, if possible
            return await Login(email, password.ConvertToUnsecureString());
        }

        /// <summary>
        ///     Autheticate the <see cref="NAvocadoClient" /> with the Avocado API using the email and password provided by the
        ///     user.
        /// </summary>
        /// <param name="email">Email provided</param>
        /// <param name="password"></param>
        /// <returns>True if authentication was successful; otherwise false</returns>
        /// <remarks>
        ///     The authentication code is based on
        ///     https://github.com/xdumaine/Avocado/blob/master/Avocado/Models/AuthClient.cs.
        /// </remarks>
        /// <exception cref="AuthenticationFailedException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<bool> Login(string email, string password)
        {
            Email = email;

            var credentials = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("password", password)
            });

            using (var response = await _httpClient.PostAsync(ApiUrlLogin, credentials))
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new BadRequestException("Bad username or password");
                }

                response.EnsureSuccessStatusCode();

                var tempCookie = response.Headers.First(x => x.Key == "Set-Cookie").Value.First();

                // TODO#100: Find a more fancy and easier to read solution (for me) to get the cookie value
                _cookieValue = tempCookie.Substring(tempCookie.IndexOf("=") + 1,
                    tempCookie.IndexOf(";") - tempCookie.IndexOf("=") - 1);

                var hashedCookie = (_cookieValue + _devKey).ToSHA256();
                _signature = _devId + ":" + hashedCookie;

                _cookieName = tempCookie;

                _httpClient.DefaultRequestHeaders.Add("X-AvoSig", _signature);

                return response.StatusCode == HttpStatusCode.OK && response.Headers.Contains("Set-Cookie");
            }
        }

        /// <summary>
        ///     Log the current <see cref="NAvocado.User" /> out
        /// </summary>
        /// <returns>True if successful; otherwise false</returns>
        public async Task<bool> Logout()
        {
            return await GetNothingAsync(ApiUrlLogout);
        }

        /// <summary>
        ///     Retrieve the <see cref="NAvocado.User" /> that is associated with the provided Email
        /// </summary>
        /// <returns><see cref="NAvocado.User" /> object</returns>
        public async Task<User> CurrentUser()
        {
            return await GetSingleAsync<User>(ApiUrlUser);
        }

        /// <summary>
        ///     Find a <see cref="NAvocado.User" /> with the provided id.
        /// </summary>
        /// <param name="id">Id of the <see cref="NAvocado.User" /> you want to find</param>
        /// <returns><see cref="NAvocado.User" /> object</returns>
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<User> User(string id)
        {
            using (var response = await _httpClient.GetAsync(ApiUrlUser + id))
            {
                response.EnsureSuccessStatusCode();

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new UserNotFoundException("User not found or inaccessible to you");
                }

                return
                    new JavaScriptSerializer().Deserialize<User[]>(await response.Content.ReadAsStringAsync())[0];
            }
        }

        /// <summary>
        ///     Retrieve the <see cref="NAvocado.Couple" /> the <see cref="NAvocado.User" /> is in.
        /// </summary>
        /// <returns><see cref="NAvocado.Couple" /> object, containing both <see cref="NAvocado.User" />s</returns>
        /// <remarks>
        ///     -   It might be smart if you immediatly call this instead of <see cref="GetCurrentUser" />, this will save a
        ///     call to the Avocado servers.
        ///     -   The second <see cref="NAvocado.User" /> item will be null if current <see cref="NAvocado.User" /> is not in a
        ///     couple.
        /// </remarks>
        public async Task<Couple> Couple()
        {
            return await SpecialMethodForCoupleStyledJSONRepresentedObjectBecauseWtf<Couple>(ApiUrlCouple);
        }

        /// <summary>
        ///     Get the last 100 activities
        /// </summary>
        /// <returns>Array of <see cref="Activity" /></returns>
        /// <remarks>If you want activities before the last 100, use <see cref="ActivitiesBefore" /></remarks>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<Activity[]> Activities()
        {
            using (var response = await _httpClient.GetAsync(ApiUrlActivities))
            {
                response.EnsureSuccessStatusCode();

                return
                    new JavaScriptSerializer().Deserialize<Activity[]>(
                        await response.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        ///     Get the last 100 activities and filter out everything but the required type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<Activity[]> ActivitiesByType(ActivityType type)
        {
            var a = await Activities();

            switch (type)
            {
                case ActivityType.Message:
                    return a.Where(i => i.Type == "message") as Activity[];
                case ActivityType.Kiss:
                    return a.Where(i => i.Type == "kiss") as Activity[];
                case ActivityType.Hug:
                    return a.Where(i => i.Type == "hug") as Activity[];
                case ActivityType.List:
                    return a.Where(i => i.Type == "list") as Activity[];
                case ActivityType.Photo:
                    return a.Where(i => i.Type == "photo") as Activity[];
                case ActivityType.Media:
                    return a.Where(i => i.Type == "media") as Activity[];
                case ActivityType.Activity:
                    return a.Where(i => i.Type == "activity") as Activity[];
                case ActivityType.Couple:
                    return a.Where(i => i.Type == "couple") as Activity[];
                case ActivityType.User:
                    return a.Where(i => i.Type == "user") as Activity[];
                case ActivityType.None:
                    return a;
                default:
                    return a;
            }
        }

        /// <summary>
        ///     Get all activities after a certain time.
        /// </summary>
        /// <param name="time"><see cref="DateTime" /> representing</param>
        /// <returns></returns>
        public async Task<Activity[]> ActivitiesAfter(DateTime time)
        {
            return await GetArrayAsync<Activity>(ApiUrlActivities + "?after=" + time.ToUnixTimestampAsLong());
        }

        /// <summary>
        ///     Get all activities before a certain time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public async Task<Activity[]> ActivitiesBefore(DateTime time)
        {
            return await GetArrayAsync<Activity>(ApiUrlActivities + "?before=" + time.ToUnixTimestampAsLong());
        }

        /// <summary>
        ///     Send a message to the Avocado API and the other use in the <see cref="NAvocado.Couple" />.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> Message(string message)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("message", message)
            });

            return await PostAsync(ApiUrlConversation, content);
        }

        /// <summary>
        ///     Send a Hug
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Hug()
        {
            return await PostAsync(ApiUrlConversationHug, null);
        }

        /// <summary>
        ///     Get all <see cref="NAvocado.List" />s.
        /// </summary>
        /// <returns></returns>
        public async Task<List[]> Lists()
        {
            return await GetArrayAsync<List>(ApiUrlLists);
        }

        /// <summary>
        ///     Get a <see cref="NAvocado.List" /> by id.
        /// </summary>
        /// <param name="id">Id of the list</param>
        /// <returns></returns>
        public async Task<List> List(string id)
        {
            return await GetSingleAsync<List>(ApiUrlLists + id);
        }

        /// <summary>
        ///     Create a new <see cref="NAvocado.List" /> with the provided name
        /// </summary>
        /// <param name="listName">Name of the <see cref="NAvocado.List"/></param>
        /// <returns></returns>
        public async Task<List> CreateList(string listName)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", listName)
            });

            return await PostAsync<List>(ApiUrlLists, content);
        }

        /// <summary>
        ///     Rename a <see cref="NAvocado.List"/>
        /// </summary>
        /// <param name="id">Id of the <see cref="NAvocado.List"/></param>
        /// <param name="newName">New name for the <see cref="NAvocado.List"/></param>
        /// <returns></returns>
        public async Task<List> RenameList(string id, string newName)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", newName)
            });

            return await PostAsync<List>(ApiUrlLists + id, content);
        }

        /// <summary>
        ///     Delete a <see cref="NAvocado.List"/> by id.
        /// </summary>
        /// <param name="id">If of the <see cref="NAvocado.List"/></param>
        /// <returns></returns>
        public async Task<bool> DeleteList(string id)
        {
            return await PostAsync(ApiUrlLists + id + "/delete", null);
        }

        #region Private Areas ( ͡° ͜ʖ ͡°)

        /// <summary>
        ///     <see cref="GetSingleAsync{T}" /> will not work for <see cref="NAvocado.Couple" />, so this method is dedicated for
        ///     the 'special' ones.
        ///     Everything is returned as array except <see cref="NAvocado.Couple" />, why is this even a thing?
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        private async Task<T> SpecialMethodForCoupleStyledJSONRepresentedObjectBecauseWtf<T>(string url)
        {
            using (var response = await _httpClient.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();

                return new JavaScriptSerializer().Deserialize<T>(await response.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        ///     Get nothing async? Por que? What is this sorcery?
        /// </summary>
        /// <param name="url"></param>
        /// <returns>True if we got nothing (status code 200/OK)!; otherwise... false???</returns>
        /// <exception cref="HttpRequestException"></exception>
        private async Task<bool> GetNothingAsync(string url)
        {
            using (var response = await _httpClient.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();

                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        /// <summary>
        ///     Get data, as a sinlge instance type thingy object stuff? ASYNC?!
        /// </summary>
        /// <typeparam name="T">The type the object should be of (english?)</typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        private async Task<T> GetSingleAsync<T>(string url)
        {
            using (var response = await _httpClient.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();

                return new JavaScriptSerializer().Deserialize<T[]>(await response.Content.ReadAsStringAsync())[0];
            }
        }

        /// <summary>
        ///     Get data, as an array and it's ASYNC!
        /// </summary>
        /// <typeparam name="T">The object type the array should be of (english?)</typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        private async Task<T[]> GetArrayAsync<T>(string url)
        {
            using (var response = await _httpClient.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();

                return new JavaScriptSerializer().Deserialize<T[]>(await response.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        ///     Post data, ASYNC! FUCK YEAH! This is a fire-and-forget style web request, we don't really care what the response is
        ///     aslong as the server told us that it was OK (200)
        /// </summary>
        /// <param name="url">The url to post to</param>
        /// <param name="content">The content to post</param>
        /// <returns>True if response is 200; otherwise false</returns>
        /// <exception cref="HttpRequestException"></exception>
        private async Task<bool> PostAsync(string url, HttpContent content)
        {
            using (var response = await _httpClient.PostAsync(url, content))
            {
                response.EnsureSuccessStatusCode();

                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        /// <summary>
        ///     Post data aswell! But we also expect something in return! Also async.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        private async Task<T> PostAsync<T>(string url, HttpContent content)
        {
            using (var response = await _httpClient.PostAsync(url, content))
            {
                response.EnsureSuccessStatusCode();

                return new JavaScriptSerializer().Deserialize<T[]>(await response.Content.ReadAsStringAsync())[0];
            }
        }

        #endregion
    }
}
