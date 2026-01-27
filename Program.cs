using System.Text.Json;
using System.Text.Json.Serialization;
using MaxApiMy.Business;
using MaxApiMy.Data;
using MaxApiMy.Common;
using Common;
using MaxApiMy.Tests;

//new Api_Tests().Test1();
//new ApiChats_Tests().Test1();
new ApiChatsMembers_Tests().AddAsync();
Console.ReadKey();