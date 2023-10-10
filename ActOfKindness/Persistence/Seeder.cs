

using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class Seeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Seeder(ApplicationDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            if (!_dbContext.Roles.Any())
            {
                await CreateRolesAsync();
            }

            if (!_dbContext.Users.Any())
            {
                await CreateAdminsAsync();
                await CreateUserAsync();
            }

            if (!_dbContext.Events.Any())
            {
                var moderatedEvents = GetModeratedEvents();
                _dbContext.Events.AddRange(moderatedEvents);
                moderatedEvents[0].Participants.AddRange(AddParticipantsToEvent(moderatedEvents[0]));

                var unmoderatedEvents = GetUnmoderatedEvents();
                _dbContext.Events.AddRange(unmoderatedEvents);

                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task CreateRolesAsync()
        {
            var roles = new[] { "Admin", "Moderator", "User" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task CreateAdminsAsync()
        {
            var admins = new List<AppUser>()
            {
                new()
                {
                    Nickname = "maciekdmd90",
                    Email = "maciekdmd90@gmail.com",
                    UserName = "Maciek",
                    EmailConfirmed = true,
                    Location = "Katowice",
                    Photos = new List<Photo?>
                    {
                        new Photo
                        {
                            IsMain = true,
                            Url = "https://res.cloudinary.com/do5wipffc/image/upload/v1692609948/770117_people_512x512_zsgurn.png",
                            Id = "1111"
                        }
                    }
                },
                new()
                {
                    Nickname = "aneta",
                    Email = "shocked.frown@gmail.com",
                    UserName = "Aneta",
                    EmailConfirmed = true,
                    Location = "Kraków",
                    Photos = new List<Photo?>
                    {
                        new Photo
                        {
                            IsMain = true,
                            Url = "https://res.cloudinary.com/do5wipffc/image/upload/v1692609948/770117_people_512x512_zsgurn.png",
                            Id = "2222"
                        }
                    }
                },
                new()
                {
                    Nickname = "marcin",
                    Email = "ripek33@gmail.com",
                    UserName = "Marcin",
                    EmailConfirmed = true,
                    Location = "Lublin",
                    Photos = new List<Photo?>
                    {
                        new Photo
                        {
                            IsMain = true,
                            Url = "https://res.cloudinary.com/do5wipffc/image/upload/v1692609948/770117_people_512x512_zsgurn.png",
                            Id = "3333"
                        }
                    }
                },
                new()
                {
                    Nickname = "tomek",
                    Email = "tomek@gmail.com",
                    UserName = "Tomek",
                    EmailConfirmed = true,
                    Location = "Białystok",
                    Photos = new List<Photo?>
                    {
                        new Photo
                        {
                            IsMain = true,
                            Url = "https://res.cloudinary.com/do5wipffc/image/upload/v1692609948/770117_people_512x512_zsgurn.png",
                            Id = "44444"
                        }
                    }
                },
                new()
                {
                    Nickname = "admin",
                    Email = "admin@test.com",
                    UserName = "Admin",
                    EmailConfirmed = true,
                    Location = "Warszawa",
                    Photos = new List<Photo?>
                    {
                        new Photo
                        {
                            IsMain = true,
                            Url = "https://res.cloudinary.com/do5wipffc/image/upload/v1692609948/770117_people_512x512_zsgurn.png",
                            Id = "55555"
                        }
                    }
                }
            };

            foreach (var admin in admins)
            {
                await _userManager.CreateAsync(admin, "ActOfKindness2023,");
                await _userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        private async Task CreateUserAsync()
        {
            var user = new AppUser()
            {
                Nickname = "user",
                Email = "user@test.com",
                UserName = "User",
                EmailConfirmed = true,
                Location = "Warszawa",
                Photos = new List<Photo?>
                {
                    new Photo
                    {
                        IsMain = true,
                        Url = "https://res.cloudinary.com/do5wipffc/image/upload/v1692609948/770117_people_512x512_zsgurn.png",
                        Id = "9999"
                    }
                }
            };

            await _userManager.CreateAsync(user, "ActOfKindness2023,");
            await _userManager.AddToRoleAsync(user, "User");
        }

        private List<Event> GetModeratedEvents()
        {
            var moderatedEvents = new List<Event>()
            {
                new()
                {
                    Id = new Guid(),
                    UserId = _userManager.FindByNameAsync("Aneta").Result!.Id,
                    Title = "Lady Jadzia - be kind to check up on her.",
                    Localization = "Poland, Malbork",
                    Description =
                        "Lady Jadzia is an old lady, living alone in her tiny old house. She is 83 years old and can barely walk. She has no living family and daily chores can be a real challange for her. If you happen to come by and ask if she needs anything, maybe just to help her with  shopping (she cannot use Glovo) or even just for a short small talk which can brighten up her day - that could really warm her heart up.\r\n We've met her during our holiday trip (we live 500km from her) and she was honsetly delighted with our help. If you have a bit of kindness in you and want to share it, you can count on lady Jadzia's gratitude.",
                    StartingDate = new DateTime(2023, 08, 28),
                    EndingDate = new DateTime(2023, 09, 20, 23, 59, 59),
                    Type = EventType.HelpNeeded,
                    IsOnline = false,
                    Image = "https://res.cloudinary.com/do5wipffc/image/upload/v1692607440/seedEvent01_i3c5y1.png",
                    IsModerated = true,
                },
                new()
                {
                    Id = new Guid(),
                    UserId = _userManager.FindByNameAsync("Marcin").Result!.Id,
                    Title = "Cleaning Augustów forest",
                    Localization = "Poland, Augustów",
                    Description =
                        "We invite you to join us in cleaning Augustów forests from all the trash left by the tourists or irresponsible locals. Our last action was a big success - almost all trails were completely wiped out from non-organic matter!",
                    StartingDate = new DateTime(2023, 09, 28),
                    EndingDate = new DateTime(2023, 09, 28, 23, 59, 59),
                    Type = EventType.HelpNeeded,
                    IsOnline = false,
                    Image = "https://res.cloudinary.com/do5wipffc/image/upload/v1692607505/seedEvent02_wlwgno.png",
                    IsModerated = true
                },
                new()
                {
                    Id = new Guid(),
                    UserId = _userManager.FindByNameAsync("Maciek").Result!.Id,
                    Title = "English webinar - A1/A2 level",
                    Localization = "Online",
                    Description =
                        "I can help anyone to start learning English completely for free. Please join my regularly scheduled webinars and I'll do my best to decompose all the rules of this language to easily chunkable bits.",
                    StartingDate = new DateTime(2023, 09, 4),
                    EndingDate = new DateTime(2023, 12, 14, 23, 59, 59),
                    Type = EventType.HelpOffer,
                    IsOnline = true,
                    Image = "https://res.cloudinary.com/do5wipffc/image/upload/v1692607441/seedEvent03_t3qfam.png",
                    IsModerated = true
                },
                new()
                {
                    Id = new Guid(),
                    UserId = _userManager.FindByNameAsync("Tomek").Result!.Id,
                    Title = "Lady Krysia tries to survive from knitting and can use your support",
                    Localization = "Szczecin",
                    Description =
                        "She doesn't want anything for free, so maybe you can find a nice pair of socks or a cap for yourself",
                    StartingDate = new DateTime(2023, 09, 4),
                    EndingDate = new DateTime(2023, 10, 12, 23, 59, 59),
                    Type = EventType.HelpNeeded,
                    IsOnline = false,
                    Image = "https://res.cloudinary.com/do5wipffc/image/upload/v1692607441/seedEvent04_ctkyij.png",
                    IsModerated = true,
                },
                new()
                {
                    Id = new Guid(),
                    UserId = _userManager.FindByNameAsync("Admin").Result!.Id,
                    Title = "Old lady needs some support",
                    Localization = "Grodzisk Mazowiecki",
                    Description =
                        "You can find her every day by the bus station and she doesn't want anything for free. If you happen to be near by, please feel free to buy her self-made bread or żurek soup in a jar.",
                    StartingDate = new DateTime(2023, 09, 20),
                    EndingDate = new DateTime(2023, 10, 21, 23, 59, 59),
                    Type = EventType.HelpNeeded,
                    IsOnline = false,
                    IsModerated = true,
                    Image = "https://res.cloudinary.com/do5wipffc/image/upload/v1692607440/seedEvent05_dn1ygf.png"
                },
            };

            return moderatedEvents;
        }

        private List<Event> GetUnmoderatedEvents()
        {
            var events = new List<Event>()
            {
                new()
                {
                    Id = new Guid(),
                    UserId = _userManager.FindByNameAsync("Aneta").Result!.Id,
                    Title = "Fardin cannot go back home",
                    Localization = "Częstochowa",
                    Description =
                        "Fardin Kazemi's truck got completely broken and he cannot go home to Iran. We need someone with good mechanical knowledge to help us fix his vehicle and help him reunite with his family.",
                    StartingDate = new DateTime(2023, 09, 05),
                    EndingDate = new DateTime(2023, 09, 24, 23, 59, 59),
                    Type = EventType.HelpNeeded,
                    IsOnline = false,
                    Image = "https://res.cloudinary.com/do5wipffc/image/upload/v1692607440/seedEvent06_wcl95w.png"
                },
                new()
                {
                    Id = new Guid(),
                    UserId = _userManager.FindByNameAsync("Marcin").Result!.Id,
                    Title = "If you're going through tough time - keep going!",
                    Localization = "Online",
                    Description =
                        "Everyone has a time of doubt and loneliness sometimes. If you're feeling down and need someone to talk-it-through, please feel free to contact me. We are in this together. You are not alone.",
                    StartingDate = new DateTime(2023, 09, 6),
                    EndingDate = new DateTime(2023, 11, 4, 23, 59, 59),
                    Type = EventType.HelpOffer,
                    IsOnline = true,
                    Image = "https://res.cloudinary.com/do5wipffc/image/upload/v1692607440/seedEvent07_m89wau.png"
                },
                new()
                {
                    Id = new Guid(),
                    UserId = _userManager.FindByNameAsync("Maciek").Result!.Id,
                    Title = "Free apples!",
                    Localization = "Grudziądz",
                    Description =
                        "Well, maybe it's not a big help, but at least you can get few additional vitamins and a healthy fiber. :) My apple trees gave me too many apples this year, so please feel free to take some with you! Enjoy and remember - an apple a day keeps the doctor away!",
                    StartingDate = new DateTime(2023, 09, 14),
                    EndingDate = new DateTime(2023, 11, 1, 23, 59, 59),
                    Type = EventType.HelpNeeded,
                    IsOnline = false,
                    Image = "https://res.cloudinary.com/do5wipffc/image/upload/v1692607440/seedEvent08_ju7pdh.png"
                },
                new()
                {
                    Id = new Guid(),
                    UserId = _userManager.FindByNameAsync("Tomek").Result!.Id,
                    Title = "First aid course for everyone",
                    Localization = "Warszawa",
                    Description =
                        "You never know when you are gonna need it, but when the time comes - you'll be glad to know what to do. I offer completely free first aid training for anyone interested! Please join us on 17th of July in Warsaw, Helping street 17.",
                    StartingDate = new DateTime(2023, 09, 17),
                    EndingDate = new DateTime(2023, 09, 17, 23, 59, 59),
                    Type = EventType.HelpOffer,
                    IsOnline = false,
                    Image = "https://res.cloudinary.com/do5wipffc/image/upload/v1692607440/seedEvent09_e8pqze.png"
                },
                new()
                {
                    Id = new Guid(),
                    UserId = _userManager.FindByNameAsync("User").Result!.Id,
                    Title = "Need a second pair of hands",
                    Localization = "Poland, Oświęcim",
                    Description =
                        "I'll have my fridge delivered on 20/07/2023, but the driver will only leave it by the door. Since my brother will be at the hospital at that time, I need someone to help me place it in the kitchen located on the 2nd floor. Beer and snacks provided! ",
                    StartingDate = new DateTime(2023, 09, 20),
                    EndingDate = new DateTime(2023, 09, 21, 23, 59, 59),
                    Type = EventType.HelpNeeded,
                    IsOnline = false,
                    Image = "https://res.cloudinary.com/do5wipffc/image/upload/v1692607441/seedEvent10_ps44z6.png"
                }
            };

            return events;
        }

        private List<EventUser> AddParticipantsToEvent(Event eventToAdd)
        {
            var eventsParticipants = new List<EventUser>()
            {
                new()
                {
                    Event = eventToAdd,
                    User = _userManager.FindByNameAsync("Marcin").Result!
                },
                new()
                {
                    Event = eventToAdd,
                    User = _userManager.FindByNameAsync("User").Result!
                }
            };

            return eventsParticipants;
        }
    }
}
