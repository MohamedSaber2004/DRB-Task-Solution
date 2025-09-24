using AutoMapper;

public class RouteProfile: Profile
{
    public RouteProfile()
    {
        CreateMap<Route, RouteDto>()
            .ForMember(dest => dest.RouteStatus, opt => opt.MapFrom(src => src.RouteStatus))
            .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver!.Name));

        CreateMap<CreatedRouteDto, Route>();
    }
}
