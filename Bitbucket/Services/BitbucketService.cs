namespace BitBucketWrapper.Bitbucket.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Atlassian.Stash;
    using Atlassian.Stash.Entities;
    using Atlassian.Stash.Helpers;

    using BitBucketWrapper.Bitbucket.Models;

    public class BitbucketService
    {
        private readonly ParallelOptions _parallelOptions = new ParallelOptions
                                                                {
                                                                    MaxDegreeOfParallelism = Environment.ProcessorCount
                                                                };

        internal readonly RequestOptions _requestOptions = new RequestOptions
                                                              {
                                                                  Limit = 99999,
                                                                  Start = 0,
                                                              };

        internal StashClient _client;

        private ConcurrentBag<StoreItem> _store;

        public ConcurrentBag<StoreItem> Store
        {
            get
            {
                if (_store == null)
                {
                    throw new InvalidOperationException("Please use InitializeStore() first");
                }

                return _store;
            }
            private set => _store = value;
        }

        public void InitializeStore()
        {
            if (_client == null)
            {
                throw new InvalidOperationException("Please use Connect() first");
            }

            Store = new ConcurrentBag<StoreItem>();

            var projects = _client.Projects.Get(_requestOptions).Result.Values.ToList();
            HandleProjects(projects);
        }

        private void HandleProjects(IEnumerable<Project> projects)
        {
            Parallel.ForEach(
                projects,
                _parallelOptions,
                project =>
                    {
                        var repositories = _client.Repositories.Get(project.Key, _requestOptions).Result.Values.ToList();
                        HandleRepositories(repositories, project);
                    });
        }

        private void HandleRepositories(IEnumerable<Repository> repositories, Project project)
        {
            foreach (var repository in repositories)
            {
                var commits = _client.Commits.Get(project.Key, repository.Slug, _requestOptions).Result.Values.ToList();
                HandleCommits(commits, project, repository);
            }
        }

        private void HandleCommits(IEnumerable<Commit> commits, Project project, Repository repository)
        {
            foreach (var commit in commits)
            {
                AddCommit(project, repository, commit);
            }
        }

        private void AddCommit(Project project, Repository repository, Commit commit)
        {
            var tuple = new StoreItem(project, repository, commit);
            Store.Add(tuple);
        }

        public void Connect(string bitbucketUrl, string username, string password)
        {
            _client = new StashClient(bitbucketUrl, username, password);
        }
    }
}