using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class Utilities
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        public static void InitDatabase(DbContextOptions contextOption)
        {
            if (_databaseInitialized)
                return;
            lock (_lock)
            {
                if (_databaseInitialized)
                    return;

                using var context = new ScreamBackend.DB.ScreamDB(contextOption);

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var user = FakeUser;
                var commentAuthor = FakeCommentAuthor;
                var screams = GetFakeScreams(user);
                var comments = GetFakeComments(screams, commentAuthor);
                context.Users.Add(user);
                context.Users.Add(commentAuthor);
                context.Screams.AddRange(screams);
                context.Comments.AddRange(comments);

                const int NUMBER_OF_USER_AND_COMMENT_AUTHOR = 2;
                int shouldEffects = screams.Count + comments.Count + NUMBER_OF_USER_AND_COMMENT_AUTHOR;
                context.SaveChanges();

                _databaseInitialized = true;
            }
        }

        private static ScreamBackend.DB.Tables.User FakeUser => new ScreamBackend.DB.Tables.User
        {
            Username = "Dvorak",
            NormalizedUsername = "DVORAK",
            Email = "dvorak@outlook.com",
            NormalizedEmail = "DVORAK@OUTLOOK.COM",
            IsAdmin = false,
            CreateDateTime = DateTime.Now,
            Avatar = ""
        };
        private static ScreamBackend.DB.Tables.User FakeCommentAuthor => new ScreamBackend.DB.Tables.User
        {
            Username = "Comment Dvorak",
            NormalizedUsername = "COMMENT DVORAK",
            Email = "Comment dvorak@outlook.com",
            NormalizedEmail = "COMMENTDVORAK@OUTLOOK.COM",
            IsAdmin = false,
            CreateDateTime = DateTime.Now,
            Avatar = ""
        };

        private static List<Scream> GetFakeScreams(User user)
        {
            const int SCREAM_COUNT = 22;
            const string CONTENT = "TEST: SCREAM CONTENT";
            List<Scream> screams = new List<Scream>(SCREAM_COUNT);
            for (int i = 1; i <= SCREAM_COUNT; i++)
            {
                string number = $"_{i}";
                screams.Add(new Scream
                {
                    Content = CONTENT + number,
                    ContentLength = CONTENT.Length + number.Length,
                    Author = user
                });
            }
            return screams;
        }

        private static List<Comment> GetFakeComments(List<Scream> screams, User commentAuthor)
        {
            const int COMMENT_COUNT_IN_SINGLE_SCREAM = 25;
            List<Comment> comments = new List<Comment>(screams.Count * COMMENT_COUNT_IN_SINGLE_SCREAM);
            foreach (var scream in screams)
            {
                for (int i = 1; i <= COMMENT_COUNT_IN_SINGLE_SCREAM; i++)
                {
                    comments.Add(new Comment
                    {
                        Scream = scream,
                        Content = "TEST: COMMENT CONTENT",
                        Author = commentAuthor
                    });
                }
            }
            return comments;
        }
    }
}
