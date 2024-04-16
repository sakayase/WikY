﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikYModels.Models;
using WikYRepositories.DTOs.Author;

namespace WikYRepositories.IRepositories
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAll();
        Task<Author> GetFromAuthorId(int AuthorId);
        Task<User> Add(AddAuthorDTO authorDTO);
    }
}