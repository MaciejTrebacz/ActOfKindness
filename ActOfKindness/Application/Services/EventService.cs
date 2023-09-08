using Application.Dtos.Event;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IContextService _contextService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPhotoRepository _photoRepository;
        private const int PageSize = 10;
        private const string EventImagePlaceHolder =
            "https://res.cloudinary.com/do5wipffc/image/upload/v1692617053/hannah-busing-Zyx1bK9mqmA-unsplash_iinfnz.jpg";

        public EventService(IEventRepository eventRepository, IMapper mapper, IContextService contextService, UserManager<AppUser> userManager,IPhotoRepository photoRepository)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _contextService = contextService;
            _userManager = userManager;
            _photoRepository = photoRepository;
        }

        public async Task<PaginatedResults<List<DetailsEventDto>>> GetModeratedEventsAsync(int pageNumber)
        {
            var eventsFromRepository = await _eventRepository.GetModeratedEventsAsync(pageNumber, PageSize);

            var eventsQuantity = _eventRepository.GetQuantityOfModeratedEventAsync().Result;

            var eventsDto = _mapper.Map<List<DetailsEventDto>>(eventsFromRepository);

            return new PaginatedResults<List<DetailsEventDto>>(eventsDto, pageNumber,
                (int)Math.Ceiling(eventsQuantity / (double)PageSize));
        }

        public async Task<List<DetailsEventDto>> GetUserEvents(string username)
        {
            var events = await _eventRepository.GetUserEventsAsync(username);
            var eventsDto = _mapper.Map<List<DetailsEventDto>>(events);

            return eventsDto;
        }

        public async Task<List<DetailsEventDto>> GetUnmoderatedEventsAsync()
        {
            var unmoderatedEvents = await _eventRepository.GetUnmoderatedEventsAsync();
            var unmoderatedEventsDto = _mapper.Map<List<DetailsEventDto>>(unmoderatedEvents);

            return unmoderatedEventsDto;
        }

        public async Task CreateEventAsync(CreateEventDto newEventDto)
        {
            if (await _eventRepository.GetEventByIdAsync(newEventDto.Id) is not null)
                throw new BadRequestException(
                    $"Event with this ID ({newEventDto.Id}) exist",
                    "Event with this ID already exist",
                    _contextService.Method,
                    _contextService.GetUserId,
                    _contextService.GetUserRole);

            var newEvent = _mapper.Map<Event>(newEventDto);

            if (string.IsNullOrWhiteSpace(newEvent.Image))
            {
                newEvent.Image = EventImagePlaceHolder;
            }

            newEvent.UserId = _contextService.GetUserId;
            var findPhoto = await _photoRepository.FindPhotoWithoutUser(newEvent.Image);

            if (findPhoto != null) newEvent.Photos.Add(findPhoto);
            await _eventRepository.CreateEventAsync(newEvent);
            await _eventRepository.SaveAsync();

            Log.Information($"Created new event ({newEvent.Id}) by {_contextService.GetUserRole} ({newEvent.UserId})");
        }

        public async Task DeleteEventAsync(Guid id)
        {
            var eventToDelete = await _eventRepository.GetEventByIdAsync(id);

            var userId = _contextService.GetUserId;
            var role = _contextService.GetUserRole;

            if (eventToDelete is null)
                throw new NotFoundException(
                    $"Event ({id}) not found",
                    "Event not found",
                    _contextService.Method,
                    userId,
                    role);

            if (eventToDelete.UserId != userId && role == "User")
                throw new ForbidException(
                    $"You are not allowed to delete event ({id})",
                    "You are not allowed to delete this event",
                    _contextService.Method,
                    userId,
                    role);

            await _eventRepository.DeleteEventAsync(id);

            Log.Information($"Deleted event ({id}) by {role} ({userId})");
        }

        public async Task<DetailsEventDto> GetEventByIdAsync(Guid id)
        {
            var eventDetails = await _eventRepository.GetEventByIdAsync(id);

            if (eventDetails is null)
                throw new NotFoundException(
                    $"Event ({id}) not found",
                    "Event not found",
                    _contextService.Method,
                    _contextService.GetUserId,
                    _contextService.GetUserRole);

            var eventDetailsDto = _mapper.Map<DetailsEventDto>(eventDetails);

            return eventDetailsDto;
        }

        public async Task UpdateEventAsync(Guid id, EditEventDto updatedEventDto)
        {
            var eventToUpdate = await _eventRepository.GetEventByIdAsync(id);
            
            var userId = _contextService.GetUserId;
            var role = _contextService.GetUserRole;

            if (eventToUpdate is null)
                throw new NotFoundException(
                    $"Event ({id}) not found",
                    "Event not found",
                    _contextService.Method,
                    userId,
                    role);

            if (eventToUpdate.UserId != userId && role == "User")
                throw new ForbidException(
                    $"You are not allowed to update event ({id})",
                    "You are not allowed to edit this event",
                    _contextService.Method,
                    userId,
                    role);

            await _eventRepository.UpdateEventAsync(id, updatedEventDto);

            Log.Information($"Updated event ({id}) by {role} ({userId})");
        }

        public async Task ModerateEventAsync(Guid id)
        {
            var rowsChanged = await _eventRepository.ModerateEventAsync(id);

            if (rowsChanged == 0)
                throw new NotFoundException(
                    $"Event ({id}) not found",
                    "Event not found",
                    _contextService.Method,
                    _contextService.GetUserId,
                    _contextService.GetUserRole);

            Log.Information($"Moderated event ({id}) by {_contextService.GetUserRole} ({_contextService.GetUserId})");
        }

        public async Task<PaginatedResults<List<DetailsEventDto>>> GetFilteredModeratedEventsAsync(EventFilter filter, int pageNumber)
        {
            var eventsFromRepository = await _eventRepository.GetFilteredModeratedEventsAsync(filter);

            var events = eventsFromRepository
                .Skip(PageSize * (pageNumber - 1))
                .Take(PageSize)
                .ToList();

            var eventsDto = _mapper.Map<List<DetailsEventDto>>(events);

            return new PaginatedResults<List<DetailsEventDto>>(eventsDto, pageNumber,
                (int)Math.Ceiling(eventsFromRepository.Count / (double)PageSize));
        }

        public async Task<List<ParticipantDto>> GetParticipantsAsync(Guid eventId)
        {
            var newEvent =  await _eventRepository.GetEventByIdAsync(eventId);

            if (newEvent is null)
                throw new NotFoundException(
                    $"Event ({eventId}) not found",
                    "Event not found",
                    _contextService.Method,
                    _contextService.GetUserId,
                    _contextService.GetUserRole);

            var listOfParticipants = new List<ParticipantDto>();
            foreach (var newEventParticipant in newEvent.Participants)
            {
                var newParticipant = new ParticipantDto(newEventParticipant.User.UserName, newEventParticipant.User.Location, "https://api.multiavatar.com/Binx Bond.png");
                listOfParticipants.Add(newParticipant);
            }
            return listOfParticipants;
        }

        public async Task JoinEventAsync(Guid eventId)
        {
            var eventToJoin = await _eventRepository.GetEventByIdAsync(eventId);

            if (eventToJoin is null)
                throw new NotFoundException($"Event ({eventId}) not found",
                    "Event not found",
                    _contextService.Method,
                    _contextService.GetUserId,
                    _contextService.GetUserRole);

            var userId =  _contextService.GetUserId;

            if (eventToJoin.Participants.Any(eu => eu.UserId == userId))
                throw new BadRequestException($"You already joined event ({eventId})",
                    "You already joined this event",
                    _contextService.Method,
                    userId,
                    _contextService.GetUserRole);

            if (eventToJoin.IsFinished)
                throw new BadRequestException($"Cannot join to event ({eventId}) because it has ended",
                    "Cannot join to event because it has ended",
                    _contextService.Method,
                    userId,
                    _contextService.GetUserRole);

            if (userId is not null)
            {
                var userWhoWantsToJoin = await _userManager.FindByIdAsync(userId);

                if (userWhoWantsToJoin is not null)
                {
                    var eventUser = new EventUser
                    {
                        Event = eventToJoin,
                        EventId = eventToJoin.Id,
                        User = userWhoWantsToJoin,
                        UserId = userWhoWantsToJoin.Id

                    };
                    eventToJoin.Participants.Add(eventUser);
                }
            }

            await _eventRepository.SaveAsync();

            Log.Information($"{_contextService.GetUserRole} ({userId}) joined to event ({eventId})");
        }

        public async Task LeaveEventAsync(Guid eventId)
        {
            var eventToLeave = await _eventRepository.GetEventByIdAsync(eventId);
            var userId = _contextService.GetUserId;

            if (eventToLeave is null)
                throw new NotFoundException($"Event ({eventId}) not found",
                    "Event not found",
                    _contextService.Method,
                    _contextService.GetUserId,
                    _contextService.GetUserRole);

            if (eventToLeave.Participants.All(eu => eu.UserId != userId))
                throw new BadRequestException($"You are not attending the event ({eventId})",
                    "You cannot leave an event you haven't joined",
                    _contextService.Method,
                    userId,
                    _contextService.GetUserRole);

            if (eventToLeave.IsFinished)
                throw new BadRequestException($"Cannot leave event ({eventId}) because it has ended",
                    "Cannot leave event because it has ended",
                    _contextService.Method,
                    userId,
                    _contextService.GetUserRole);

            if (userId is not null)
            {
                var userWhoWantsToLeave = await _userManager.FindByIdAsync(userId);

                if (userWhoWantsToLeave is not null)
                {
                    var eventUser = eventToLeave.Participants.FirstOrDefault(p => p.UserId == userId);
                    eventToLeave.Participants.Remove(eventUser);
                }
            }

            await _eventRepository.SaveAsync();

            Log.Information($"{_contextService.GetUserRole} ({userId}) has left event ({eventId})");
        }
    }
}
