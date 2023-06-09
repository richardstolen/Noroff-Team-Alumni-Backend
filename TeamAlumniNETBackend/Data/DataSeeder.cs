﻿using TeamAlumniNETBackend.Models;
using static System.Net.WebRequestMethods;

namespace TeamAlumniNETBackend.Data
{
    public class DataSeeder
    {
        public static List<User> GetUsers()
        {
            List<User> createUsers = new List<User>();
            createUsers.Add(new User()
            {
                UserId = Guid.NewGuid(),
                UserName = "Richardinho",
                Image = "https://upload.wikimedia.org/wikipedia/commons/a/a5/Ricardinho_on_Benfica_%28cropped%29.jpg",
                Status = "Attending Experis Academy courses at Noroff",
                Bio = "Love Futsal and footbal",
                FunFact = "never watched a whole footballmatch",
                Posts = null,
                Groups = null,
                Events = null,
                Topics = null,
            });

            createUsers.Add(new User()
            {
                UserId = Guid.NewGuid(),
                UserName = "Kjetilinho",
                Image = "https://resources.premierleague.com/photos/2023/01/30/46dfc1c6-ccfd-4ad5-8d5a-79a6eceee104/Weghorst-Man-Utd.jpg?width=930&height=620",
                Status = "Attending Experis Academy courses at Noroff",
                Bio = "Striker at Manchester United",
                FunFact = "Almost never score",
            });
            return createUsers;
        }
        public static List<Group> GetGroups()
        {
            List<Group> createGroups = new List<Group>();
            createGroups.Add(new Group()
            {
                GroupId = 1,
                CreatedBy = null,
                Name = "Experis",
                Description = "Group for members of Experis",

            });
            return createGroups;
        }
        public static List<Topic> GetTopics()
        {
            List<Topic> createTopics = new List<Topic>();
            createTopics.Add(new Topic()
            {
                TopicId = 1,
                Name = "Football",
                Description = "Topic for people who love football",
            });
            return createTopics;
        }

        public static List<Event> GetEvents()
        {
            List<Event> createEvents = new List<Event>();
            createEvents.Add(new Event()
            {
                EventId = 1,
                UserId = null,
                Description = "Football game"
            });
            return createEvents;
        }

        public static List<Post> GetPosts()
        {
            List<Post> createPosts = new List<Post>();
            createPosts.Add(new Post()
            {
                PostId = 1,
                UserId = null,
                Title = "Footbal Match",
                Body = "Invite to all who like football to watch the match",
                LastUpdate = new DateTime(),
                TargetPost = null,
                TargetUser = null,
                TargetGroup = null,
                TargetEvent = null,
                TargetTopic = null
            });
            return createPosts;
        }
    }
}

