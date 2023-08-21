﻿using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.BackgroundTasks;
using Persistence.Repositories;
using Quartz;

namespace Persistence.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistence(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ActOfKindness")));
            service.AddScoped<IEventRepository, EventRepository>();
            service.AddScoped<ICommentRepository, CommentRepository>();
            service.AddScoped<IPhotoRepository, PhotoRepository>();
            service.AddScoped<Seeder>();
            service.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
            });
            service.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });
            service.ConfigureOptions<ArchiveOldEventsConfigure>();
        }
    }
}
