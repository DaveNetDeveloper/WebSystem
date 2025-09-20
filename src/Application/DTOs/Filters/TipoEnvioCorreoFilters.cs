﻿using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class TipoEnvioCorreoFilters : IFilters<TipoEnvioCorreo>
    {
        public Guid? Id { get; set; }
        public string? Nombre { get; set; } 
        public bool? Activo { get; set; }  
    }
}