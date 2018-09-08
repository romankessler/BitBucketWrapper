namespace BitBucketWrapper.Bitbucket.Extensions
{
    using System.Linq;

    using Atlassian.Stash.Entities;

    using BitBucketWrapper.Bitbucket.Models;
    using BitBucketWrapper.Bitbucket.Services;

    public static class StoreItemExtension
    {
        public static Branch InitializeBranch(this StoreItem item, BitbucketService bitbucketService)
        {
            var branches = bitbucketService._client.Branches.GetByCommitId(
                item.Project.Key,
                item.Repository.Slug,
                item.Commit.Id,
                bitbucketService._requestOptions).Result.Values;
            return branches.Single();
        }
    }
}