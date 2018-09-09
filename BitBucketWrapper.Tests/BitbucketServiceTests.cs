namespace BitBucketWrapper.Tests
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    using BitBucketWrapper.Bitbucket.Services;

    using NUnit.Framework;

    [TestFixture]
    public class BitbucketServiceTests
    {
        [Test]
        public void BitBucketService_InitializeStore_AllBranches()
        {
            var username = "";
            var password = "";
            var bitbucketUrl = "";

            // arrange
            var bitbucketService = new BitbucketService();
            bitbucketService.Connect(bitbucketUrl, username, password);

            // act
            bitbucketService.InitializeStore();

            var orderByDescending = bitbucketService.Store.Where(x => x.Project.Key == "SE").OrderByDescending(x => x.Commit.AuthorTimestamp);
            var storeCount = bitbucketService.Store.Count;

            // assert
            // TODO (ROK): 
        }
    }
}