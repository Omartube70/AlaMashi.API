using Application.Orders.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using Application.Users.Commands;
using Application.Users.DTOs;
using Application.Users.Events;
using Domain.Common;
using Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Commands
{

    //public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    //{
    //    private readonly IOrderRepository _orderRepository;


    //    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    //    {
    //        _orderRepository = orderRepository;
    //    }

    //    public async Task<UserDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    //    {




    //        User newUser = await User.CreateAsync(request.UserName, request.Email, request.Phone, hashedPassword);

    //        await _userRepository.AddUserAsync(newUser);

    //        var notification = new UserCreatedNotification(newUser.Email, newUser.UserName);

    //        await _mediator.Publish(notification, cancellationToken);

    //        return new UserDto
    //        {
    //            UserId = newUser.UserID,
    //            UserName = newUser.UserName,
    //            Email = newUser.Email,
    //            Phone = newUser.Phone
    //        };
    //    }
    //}
}
