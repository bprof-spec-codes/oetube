﻿using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Extensions;
using OeTube.Domain.Repositories;
using OeTube.Entities;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace OeTube.Application
{

    public class OeTubeUserAppService : ReadOnlyAppService<OeTubeUser, OeTubeUserDto, OeTubeUserItemDto, Guid, PagedAndSortedResultRequestDto>,
                                IUpdateAppService<UpdateOeTubeUserDto, Guid>
    {
        private readonly IOeTubeUserRepository _users;

        public OeTubeUserAppService(IOeTubeUserRepository users) : base(users)
        {
            _users = users;
        }
        public async Task<UpdateOeTubeUserDto> UpdateAsync(Guid id, UpdateOeTubeUserDto input)
        {
            CurrentUser.CheckCreator(id);
            var user = await _users.GetAsync(id);
            user.SetName(input.Name)
                .SetAboutMe(input.AboutMe);
            await _users.UpdateAsync(user, true);
            return input;
        }


    }
}
