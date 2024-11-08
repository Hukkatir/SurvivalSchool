﻿using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IForumPostService
    {
        Task<List<ForumPost>> GetAll();
        Task<ForumPost> GetById(int id);
        Task Create(ForumPost model);
        Task Update(ForumPost model);
        Task Delete(int id);
    }
}