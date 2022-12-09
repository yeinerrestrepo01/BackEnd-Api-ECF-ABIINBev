﻿using AutoMapper;
using ECF.Core.applications.Core.Interfaces;
using ECF.Core.applications.Core.Interfaces.EventosSap;
using ECF.Core.Entities.Dto;

namespace ECF.Core.applications.Core.Implementaciones.EventosSap
{
    public class AccionesSap : IAccionesSap
    {
        private readonly IDocumentoOriginalNCFManager _documentoOriginalNCFManager;

        public readonly IMapper _mapper;
        public AccionesSap(IDocumentoOriginalNCFManager documentoOriginalNCFManager, IMapper mapper)
        {
            _documentoOriginalNCFManager = documentoOriginalNCFManager;
            _mapper = mapper;   
        }


        /// <summary>
        /// Metodo para obetner el lisatdo de documentops correcion procesado para envio a sap
        /// </summary>
        /// <returns></returns>
        public List<DocumentoOriginalNCFDto> DocumentoOriginalNCF()
        {
            var consultaDocumentoOriginalNCF = _documentoOriginalNCFManager.GetAll().AsQueryable();
            return _mapper.Map<List<DocumentoOriginalNCFDto>>(consultaDocumentoOriginalNCF.ToList());
        }
    }
}
