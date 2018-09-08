namespace BitBucketWrapper.Bitbucket.Models
{
    using Atlassian.Stash.Entities;

    public class StoreItem
    {
        public StoreItem(Project project, Repository repository, Commit commit)
        {
            Project = project;
            Repository = repository;
            Commit = commit;
        }

        public Project Project { get; set; }

        public Repository Repository { get; set; }

        public Branch Branch { get; set; }

        public Commit Commit { get; set; }
    }
}