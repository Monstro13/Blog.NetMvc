﻿using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Model.Context;
using Model.Helpers;
using Model.Models;

namespace Model.Configuration
{
    internal sealed class Configuration : DbMigrationsConfiguration<BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BlogContext context)
        {
            var contacts = new Contact[]
                {
                    new Contact() {ContactId = 1, GroupId = 1, Type = "Skype", Value = "mySkype"},
                    new Contact() {ContactId = 2, GroupId = 1, Type = "Phone", Value = "myPhone"},
                    new Contact() {ContactId = 3, GroupId = 1, Type = "Adress", Value = "myAdress"},
                    new Contact() {ContactId = 4, GroupId = 1, Type = "Email", Value = "myEmail"}, 
                };

            var group = new GroupContact() { GroupContactId = 1, Title = "Main"};

            var categories = new UserCategory[]
                {
                    new UserCategory {UserCategoryId = 1,Title = "general", Description = "general group", Code = "1"}, 
                    new UserCategory {UserCategoryId = 2,Title = "a-91", Description = "general group", Code = "1"},
                    new UserCategory {UserCategoryId = 3,Title = "a-101", Description = "general group", Code = "1"},
                    new UserCategory {UserCategoryId = 4,Title = "m-61", Description = "general group", Code = "1"},
                    new UserCategory {UserCategoryId = 5,Title = "a-100", Description = "general group", Code = "1"},
                    new UserCategory {UserCategoryId = 6,Title = "b1", Description = "general group", Code = "1"},
                };

            var users = new User[]
                {
                    new User{UserId = 1, UserCategoryId = 1, Sex = true, SecondName = "Bond", Password = "007".ComputeHash(), Login = "Agent", Karma = 0, IsDeleted = false, IsBlocked = false, IsAdmin = false, IsActivate = true, FirstName = "James", Email = "jamesBond@mail.ru", DateRegistration = DateTime.Now }, 
                    new User{UserId = 2, UserCategoryId = 1, Sex = true, SecondName = "At", Password = "100".ComputeHash(), Login = "Admin", Karma = 0, IsDeleted = false, IsBlocked = false, IsAdmin = true, IsActivate = true, FirstName = "Karl", Email = "KarlKorral@mail.ru", DateRegistration = DateTime.Now }
                };

            var tags = new HashTag[]
                {
                    new HashTag{HashTagId = 1, Title = "Кот", Weigth = 0},
                    new HashTag{HashTagId = 2, Title = "котики", Weigth = 0},
                    new HashTag{HashTagId = 3, Title = "вертолет", Weigth = 0},
                    new HashTag{HashTagId = 4, Title = "украина", Weigth = 0},
                    new HashTag{HashTagId = 5, Title = "Революция", Weigth = 0},
                    new HashTag{HashTagId = 6, Title = "майдан", Weigth = 0},
                    new HashTag{HashTagId = 7, Title = "европа", Weigth = 0},
                    new HashTag{HashTagId = 8, Title = "парик", Weigth = 0},
                    new HashTag{HashTagId = 9, Title = "контакт", Weigth = 0},
                    new HashTag{HashTagId = 10, Title = "наукота", Weigth = 0},
                    new HashTag{HashTagId = 11, Title = "интерестности", Weigth = 0},
                    new HashTag{HashTagId = 12, Title = "мыло", Weigth = 0},
                    new HashTag{HashTagId = 13, Title = "мыльница", Weigth = 0},
                    new HashTag{HashTagId = 14, Title = "зеркало", Weigth = 0},
                    new HashTag{HashTagId = 15, Title = "пранки", Weigth = 0},
                    new HashTag{HashTagId = 16, Title = "пранк", Weigth = 0},
                    new HashTag{HashTagId = 17, Title = "бег", Weigth = 0},
                    new HashTag{HashTagId = 18, Title = "пшеее", Weigth = 0},
                    new HashTag{HashTagId = 19, Title = "польша", Weigth = 0},
                    new HashTag{HashTagId = 20, Title = "восстание", Weigth = 0},
                    new HashTag{HashTagId = 21, Title = "юмор", Weigth = 0},
                    new HashTag{HashTagId = 22, Title = "пикабу", Weigth = 0},
                    new HashTag{HashTagId = 23, Title = "кино", Weigth = 0},
                    new HashTag{HashTagId = 24, Title = "новинка", Weigth = 0},
                    new HashTag{HashTagId = 25, Title = "новости", Weigth = 0},
                    new HashTag{HashTagId = 26, Title = "ютуб", Weigth = 0},
                    new HashTag{HashTagId = 27, Title = "работа", Weigth = 0},
                    new HashTag{HashTagId = 28, Title = "лень", Weigth = 0},
                    new HashTag{HashTagId = 29, Title = "учеба", Weigth = 0},
                    new HashTag{HashTagId = 30, Title = "стандарты", Weigth = 0},
                };


            const string text = "Lorem Ipsum - это текст-рыба, часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной рыбой для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн. Его популяризации в новое время послужили публикация листов Letraset с образцами Lorem Ipsum в 60-х годах и, в более недавнее время, программы электронной вёрстки типа Aldus PageMaker, в шаблонах которых используется Lorem Ipsum.";

            var rubrics = new Rubric[]
                {
                    new Rubric{RubricId = 1, Title = "Kino", Description = "About Kino with Kino"},
                    new Rubric{RubricId = 2, Title = "More", Description = "About More with More"},
                    new Rubric{RubricId = 3, Title = "UFO", Description = "About UFO with UFO"}, 
                };

            var testKommentars = new Kommentar[]
                {
                    new Kommentar{KommentarId = 1, UserId = 1, Rating = 0, Message = text, IsSpam = false, IsDeleted = false, IsConfirm = true, DateCreation = DateTime.Now}, 
                     new Kommentar{KommentarId = 2, UserId = 1,Rating = 0, Message = text, IsSpam = false, IsDeleted = false, IsConfirm = true, DateCreation = DateTime.Now}, 
                      new Kommentar{KommentarId = 3, UserId = 1,Rating = 0, Message = text, IsSpam = false, IsDeleted = false, IsConfirm = true, DateCreation = DateTime.Now}, 
                       new Kommentar{KommentarId = 4, UserId = 1,Rating = 0, Message = text, IsSpam = false, IsDeleted = false, IsConfirm = true, DateCreation = DateTime.Now}, 
                        new Kommentar{KommentarId = 5, UserId = 1,Rating = 0, Message = text, IsSpam = false, IsDeleted = false, IsConfirm = true, DateCreation = DateTime.Now}, 
                         new Kommentar{KommentarId = 6, UserId = 1,Rating = 0, Message = text, IsSpam = false, IsDeleted = false, IsConfirm = true, DateCreation = DateTime.Now}, 
                };

            var testHashTags = new HashTag[]
                {
                    new HashTag{HashTagId = 1, Title = "Кот", Weigth = 0},
                    new HashTag{HashTagId = 2, Title = "котики", Weigth = 0},
                    new HashTag{HashTagId = 3, Title = "вертолет", Weigth = 0},
                    new HashTag{HashTagId = 4, Title = "украина", Weigth = 0},
                    new HashTag{HashTagId = 5, Title = "Революция", Weigth = 0},
                };

            var testLinks = new Link[]
                {
                    new Link{LinkId = 1, Title = "AAA", Value = "vk.com"},
                    new Link{LinkId = 2, Title = "HET", Value = "am.ru"}
                };

            var fileCAtegories = new FileCategory[]
                {
                    new FileCategory{FileCategoryId = 1, Title = "Image"},
                    new FileCategory{FileCategoryId = 2, Title = "Video"}, 
                };

            var typesFiles = new FileType[]
                {
                    new FileType{FileTypeId = 1, TypeId = 1, Format = "png"},
                    new FileType{FileTypeId = 2, TypeId = 1, Format = "jpg"},
                };

            var files = new AttachFile[]
                {
                    new AttachFile{AttachFileId = 1, Title = "test title", FilePath = "/Images/Posts/nemo.jpg", FileTypeId = 2},
                    new AttachFile{AttachFileId = 2, Title = "test title", FilePath = "/Images/logoFocus.png", FileTypeId = 1},
                    new AttachFile{AttachFileId = 3, Title = "test title", FilePath = "/Images/logoFocus.png", FileTypeId = 1},
                    new AttachFile{AttachFileId = 4, Title = "test title", FilePath = "/Images/logoFocus.png", FileTypeId = 1}
                };

            var news = new Post[]
                {
                    new Post{PostId = 1, UserId = 1, UserCategoryId = 1, DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 2, UserId = 2,UserCategoryId = 1, DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 3, UserId = 1,UserCategoryId = 1, DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 4, UserId = 1,UserCategoryId = 1, DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 5, UserId = 1, UserCategoryId = 1,DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 6, UserId = 1,UserCategoryId = 1, DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 7, UserId = 1,UserCategoryId = 1, DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 8, UserId = 1, UserCategoryId = 1,DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 9, UserId = 1,UserCategoryId = 1, DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 10, UserId = 1,UserCategoryId = 1, DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 11, UserId = 1,UserCategoryId = 1, DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 12, UserId = 1,UserCategoryId = 1, DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 13, UserId = 1,UserCategoryId = 1, DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 14, UserId = 1,UserCategoryId = 1, DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files},
                    new Post{PostId = 15, UserId = 1,UserCategoryId = 1, DateCreation = DateTime.Now, IsConfirm = true, IsDeleted = false, IsSpam = false, Title = "Test Post", Message = text, Rating = 0, RubricId = 1, HashTags = testHashTags, Links = testLinks, Files = files}
                };

            context.Contacts.AddOrUpdate(contacts);
            context.GroupContacts.AddOrUpdate(group);
            context.UserCategories.AddOrUpdate(categories);
            context.Users.AddOrUpdate(users);
            context.HashTags.AddOrUpdate(tags);
            context.Rubrics.AddOrUpdate(rubrics);
            context.Links.AddOrUpdate(testLinks);
            context.FileTypes.AddOrUpdate(typesFiles);
            context.AttachFiles.AddOrUpdate(files);
            context.FileCategories.AddOrUpdate(fileCAtegories);
            context.Posts.AddOrUpdate(news);
        }
    }
}