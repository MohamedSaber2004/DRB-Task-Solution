using AutoMapper;

public class DriverProfile: Profile
{
    public DriverProfile()
    {
        CreateMap<CreatedDriverDto, Driver>()
            .ReverseMap();

        CreateMap<RouteHistory, HistoryRecordDto>()
             .ForMember(dest => dest.StartLocation, opt => opt.MapFrom(src => src.StartLocation))
             .ForMember(dest => dest.EndLocation, opt => opt.MapFrom(src => src.EndLocation))
             .ForMember(dest => dest.CompletedOn, opt => opt.MapFrom(src => src.CompletedOn))
             .ForMember(dest => dest.Distance, opt => opt.Ignore());

        CreateMap<Driver, DriverHistoryDto>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
             .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.RouteHistories))
             .AfterMap((src, dest, context) => {
                 foreach (var historyRecord in dest.History)
                 {
                     var matchingRoute = src.Routes?.FirstOrDefault(r =>
                         r.StartLocation == historyRecord.StartLocation &&
                         r.EndLocation == historyRecord.EndLocation);
                     historyRecord.Distance = matchingRoute?.Distance ?? 0;
                 }
             });
    }
}

