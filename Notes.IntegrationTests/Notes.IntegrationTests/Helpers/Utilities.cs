﻿using AutoFixture;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Notes.Constants;
using Notes.Domain.Entities;
using Notes.Infrastructure.Data;

namespace Notes.IntegrationTests.Helpers
{
    public static class Utilities
    {
        private static IFixture _fixture = new Fixture();
        private static List<Note> seedNotes = _fixture.Build<Note>()
                .Without(x => x.Id)
                .Without(x => x.UserId)
                .Do(x => x.Id = ObjectId.GenerateNewId().ToString())
                .Do(x => x.UserId = ObjectId.GenerateNewId().ToString())
                .CreateMany(3).ToList();

        public static void ReinitializeDbForTests(IMongoDatabase db, MongoDbSettings mongoDbSettings)
        {
            try
            {
                db.DropCollection(mongoDbSettings.NotesCollectionName);
                var notes = db.GetCollection<Note>(mongoDbSettings.NotesCollectionName);
                notes.InsertMany(seedNotes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void ReinitializeDb(CustomWebApplicationFactory<Program> factory)
        {
            using (var scope = factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<IMongoDatabase>();

                var configuration = scopedServices.GetRequiredService<IConfiguration>();
                var mongoDbSettings = configuration.GetSection(ConfigConstants.MongoDbSettings).Get<MongoDbSettings>();

                Utilities.ReinitializeDbForTests(db, mongoDbSettings);
            }
        }

        public static List<Note> GetSeedingNotes()
        {
            return seedNotes;
        }

    }
}
