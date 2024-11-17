using Microsoft.Playwright;
using NUnit.Framework;
using System.Text.Json;

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
            //The endpoint for creating posts does not actually create the new post, check that the new post doesn't exist
            await Expect(retrieveNewPost).Not.ToBeOKAsync();
        }

        [Test]
        public async Task PUT_Existing_Post()
        {
            //Put is similar to Patch, but will replace the entire data object (and null fields not present)
            Dictionary<string, object> updateData = new Dictionary<string, object>();
            updateData.Add("title", "Updated post title");
            updateData.Add("body", "This is the updated body");
            updateData.Add("userId", 1);

            var updatedPost = await Request.PutAsync("/posts/1", new() { DataObject = updateData });

            await Expect(updatedPost).ToBeOKAsync();
            var updatedBody = await updatedPost.JsonAsync();
            Assert.IsNotNull(updatedBody);

            var retrieveExistingPost = await Request.GetAsync("/posts/1");
            var getBody = await retrieveExistingPost.JsonAsync();
            //The endpoint for updating Posts does not change the post, check that the original post does not match
            Assert.That(getBody.Value, Is.Not.EqualTo(updatedBody.Value));
        }

        [Test]
        public async Task PATCH_Existing_Post()
        {
            //Patch is similar to Put, but will only replace certain data
            Dictionary<string, object> updateData = new Dictionary<string, object>();
            updateData.Add("title", "Updated post title");

            var updatedPost = await Request.PatchAsync("/posts/1", new() { DataObject = updateData });

            await Expect(updatedPost).ToBeOKAsync();
            var updatedBody = await updatedPost.JsonAsync();
            string jsonString = JsonSerializer.Serialize(updatedBody);
            Assert.IsNotNull(updatedBody);
            Assert.That(jsonString.Contains("Updated post title"));

            var retrieveExistingPost = await Request.GetAsync("/posts/1");
            var getBody = await retrieveExistingPost.JsonAsync();
            //The endpoint for patching Posts does not change the post, check that the original post does not match
            Assert.That(getBody.Value, Is.Not.EqualTo(updatedBody.Value));
        }

        [Test]
        public async Task DELETE_Existing_Post()
        {
            var deletedPost = await Request.DeleteAsync("/posts/1");

            await Expect(deletedPost).ToBeOKAsync();
            var body = await deletedPost.JsonAsync();
            Assert.IsNotNull(body);

            var retrieveExistingPost = await Request.GetAsync("/posts/1");
            //The endpoint for deleting posts does not actually delete the new post, check that the deleted post isn't deleted
            await Expect(retrieveExistingPost).ToBeOKAsync();
        }

        [TearDown]
        public async Task TearDownAPITesting()
        {
            await Request.DisposeAsync();
        }
    }
}
