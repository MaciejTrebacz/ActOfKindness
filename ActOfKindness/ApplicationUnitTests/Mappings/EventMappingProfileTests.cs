using System.Globalization;
using AutoMapper;
using Application.Dtos.Event;
using Application.Dtos.Profile;
using Application.Dtos.User;
using Application.Mappings;
using Domain.Models;

namespace ApplicationUnitTests.Mappings
{
    public class EventMappingProfileTests
    {
        private IMapper _mapper;
        private MapperConfiguration _configuration;
        private const string UserName = "UserName";
        private const string NickName = "NickName";
        private readonly List<Photo> _photos = new List<Photo>()
        {
            new Photo()
            {
                IsMain = true,
                Url = "http://sampleUrl.com"
            }
        };

        [SetUp]
        public void Setup()
        {
            _configuration = new MapperConfiguration(cfg => cfg.AddProfile<EventMappingProfile>());
            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ConfigurationTest()
        {
            _configuration.AssertConfigurationIsValid();

            Assert.Pass();
        }

        [Test]
        public void Event_To_DetailsEventDto_MappingIsValid()
        {
            var source = new Event()
            {
                CreatedBy = new AppUser()
                {
                    UserName = UserName,
                    Nickname = NickName
                }
            };

            var destination = _mapper.Map<DetailsEventDto>(source);

            Assert.AreEqual(source.CreatedBy.Nickname, destination.CreatedBy.Nickname);
            Assert.AreEqual(source.CreatedBy.UserName, destination.CreatedBy.Username);
        }

        [Test]
        public void AppUser_To_ProfileDto_MappingIsValid()
        {
            var source = new AppUser()
            {
                Photos = _photos
            };

            var destination = _mapper.Map<ProfileDto>(source);

            Assert.AreEqual(source.Photos.FirstOrDefault(x => x.IsMain).Url, destination.MainPhotoUrl);
        }

        [Test]
        public void AppUser_To_UserDto_MappingIsValid()
        {
            var source = new AppUser()
            {
                Photos = _photos
            };

            var destination = _mapper.Map<UserDto>(source);

            Assert.AreEqual(source.Photos.FirstOrDefault(x => x.IsMain).Url, destination.MainPhotoUrl);
        }

        [Test]
        public void CreateEventDto_To_Event_MappingIsValid()
        {
            var source = new CreateEventDto(new Guid(), null, false, null, null, "11/08/2023", "12/08/2023", null, null, EventType.HelpOffer, null);
            var destination = _mapper.Map<Event>(source);

            Assert.AreEqual(DateTime.ParseExact(source.StartingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture), destination.StartingDate);
            Assert.AreEqual(DateTime.ParseExact(source.EndingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Add(new TimeSpan(23, 59, 59)), destination.EndingDate);
        }
    }
}
