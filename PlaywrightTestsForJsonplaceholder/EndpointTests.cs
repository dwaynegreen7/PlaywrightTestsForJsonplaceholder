using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightTestsForJsonplaceholder
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class EndpointTests : PlaywrightTest
    {
        private IAPIRequestContext Request = null!;

        [SetUp]
        public async Task SetUpAPITesting()
        {
            await CreateAPIRequestContext();
        }

        private async Task CreateAPIRequestContext()
        {
            Request = await this.Playwright.APIRequest.NewContextAsync(new()
            {
                // All requests we send go to this API endpoint.
                BaseURL = "https://jsonplaceholder.typicode.com/",
                //ExtraHTTPHeaders = headers,
            });
        }

        [Test]
        public async Task GET_Posts_Not_Null()
        {
            var posts = await Request.GetAsync("/posts/");
            await Expect(posts).ToBeOKAsync();
            var body = await posts.JsonAsync();
            Assert.IsNotNull(body);
        }

        [Test]
        public async Task GET_Comments_Not_Null()
        {
            var comments = await Request.GetAsync("/comments/");
            await Expect(comments).ToBeOKAsync();
            var body = await comments.JsonAsync();
            Assert.IsNotNull(body);
        }

        [Test]
        public async Task GET_Albums_Not_Null()
        {
            var albums = await Request.GetAsync("/albums/");
            await Expect(albums).ToBeOKAsync();
            var body = await albums.JsonAsync();
            Assert.IsNotNull(body);
        }

        [Test]
        public async Task GET_Photos_Not_Null()
        {
            var photos = await Request.GetAsync("/photos/");
            await Expect(photos).ToBeOKAsync();
            var body = await photos.JsonAsync();
            Assert.IsNotNull(body);
        }

        [Test]
        public async Task GET_Todos_Not_Null()
        {
            var todos = await Request.GetAsync("/todos/");
            await Expect(todos).ToBeOKAsync();
            var body = await todos.JsonAsync();
            Assert.IsNotNull(body);
        }

        [Test]
        public async Task GET_Users_Not_Null()
        {
            var users = await Request.GetAsync("/users/");
            await Expect(users).ToBeOKAsync();
            var body = await users.JsonAsync();
            Assert.IsNotNull(body);
        }

        [Test]
        public async Task POST_New_Post()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Title", "New post title");
            data.Add("Body", "This is the body");
            data.Add("UserId", 1);

            var newPost = await Request.PostAsync("/posts/", new() { DataObject = data });

            await Expect(newPost).ToBeOKAsync();
            var body = await newPost.JsonAsync();
            Assert.IsNotNull(body);

            var retrieveNewPost = await Request.GetAsync("/posts/101");
            await Expect(retrieveNewPost).Not.ToBeOKAsync();
        }

        [TearDown]
        public async Task TearDownAPITesting()
        {
            await Request.DisposeAsync();
        }
    }
}
