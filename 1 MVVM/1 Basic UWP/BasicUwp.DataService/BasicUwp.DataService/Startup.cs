using System;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;
using System.Threading.Tasks;
using BasicUwp.DataService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BasicUwp.DataService {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services) {
            services.AddDbContext<DataContext>(opt =>
                opt.UseInMemoryDatabase("ContactDatabase"));
            services.AddMvc();

            var serviceProvider = services.BuildServiceProvider();
            SeedData(serviceProvider.GetService<DataContext>());
            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvc();
        }

        public static void SeedData(DataContext context) {
            context.Contacts.Add(new Contact {
                FirstName = "Theodore", LastName = "Humphrey",
                Birthday = new DateTime(1984, 1, 5),
                Link = "http://facebook.com/th", Avatar = "/images/1.jpg",
                Message =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            });
            context.Contacts.Add(new Contact {
                FirstName = "Otis", LastName = "Will",
                Birthday = new DateTime(1968, 7, 1),
                Link = "http://facebook.com/ow", Avatar = "/images/2.jpg",
                Message =
                    "Etiam egestas eros et sapien fringilla, et molestie nisi bibendum."
            });
            context.Contacts.Add(new Contact {
                FirstName = "Keith", LastName = "Juliana",
                Birthday = new DateTime(1984, 6, 7),
                Link = "http://facebook.com/kj", Avatar = "/images/3.jpg",
                Message = "Duis ac hendrerit mi."
            });
            context.Contacts.Add(new Contact {
                FirstName = "Bennett", LastName = "Louise",
                Birthday = new DateTime(1948, 1, 8),
                Link = "http://facebook.com/bl", Avatar = "/images/4.jpg",
                Message =
                    "Ut euismod, ex vitae sodales consequat, urna sapien sollicitudin dolor, accumsan feugiat nulla nunc laoreet lectus."
            });
            context.Contacts.Add(new Contact {
                FirstName = "Leila", LastName = "Bartholomew",
                Birthday = new DateTime(1982, 1, 7),
                Link = "http://facebook.com/lb", Avatar = "/images/5.jpg",
                Message = "Praesent sagittis nec mi sed accumsan."
            });
            context.Contacts.Add(new Contact {
                FirstName = "Venus", LastName = "Edward",
                Birthday = new DateTime(1968, 12, 18),
                Link = "http://facebook.com/ve", Avatar = "/images/6.jpg",
                Message =
                    "Maecenas nunc odio, cursus at dui sit amet, pulvinar aliquam nibh."
            });
            context.Contacts.Add(new Contact {
                FirstName = "Victoria", LastName = "Webster",
                Birthday = new DateTime(1978, 11, 8),
                Link = "http://facebook.com/vw", Avatar = "/images/7.jpg",
                Message = "Nam ultricies ut nisi vel convallis."
            });
            context.Contacts.Add(new Contact {
                FirstName = "Paul", LastName = "Malan",
                Birthday = new DateTime(1994, 12, 24),
                Link = "http://facebook.com/pm", Avatar = "/images/8.jpg",
                Message = "Mauris mollis neque in massa porttitor fringilla."
            });
            context.Contacts.Add(new Contact {
                FirstName = "Julius", LastName = "Rutherford",
                Birthday = new DateTime(1992, 1, 25),
                Link = "http://facebook.com/jr", Avatar = "/images/9.jpg",
                Message = "Sed aliquet viverra tortor quis dignissim."
            });
            context.Contacts.Add(new Contact {
                FirstName = "Wendy", LastName = "Hume",
                Birthday = new DateTime(1983, 1, 14),
                Link = "http://facebook.com/wh", Avatar = "/images/10.jpg",
                Message =
                    "Interdum et malesuada fames ac ante ipsum primis in faucibus."
            });
            context.Contacts.Add(new Contact {
                FirstName = "Verne", LastName = "Geordie",
                Birthday = new DateTime(1983, 12, 8),
                Link = "http://facebook.com/vg", Avatar = "/images/11.jpg",
                Message =
                    "Aenean et arcu in ante ornare facilisis ac sed velit."
            });
            context.Contacts.Add(new Contact {
                FirstName = "Sophia", LastName = "Elinor",
                Birthday = new DateTime(2000, 7, 18),
                Link = "http://facebook.com/se", Avatar = "/images/12.jpg",
                Message = "Nam varius convallis tristique."
            });
            context.SaveChanges();
        }
    }
}