using ECF.Core.Entities.Entity;
using Newtonsoft.Json;
using ServiceReference1;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Xml;

namespace ECF.Core.applications.Base
{
    public class IntegracionSap
    {
       public ChannelFactory<zws_posteo_correcciones> _channelFactory;
        zws_posteo_correcciones _zws_Posteo_Correcciones;
        public IntegracionSap()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;


            _channelFactory = new ChannelFactory<zws_posteo_correcciones>(binding, new EndpointAddress("https://cnddosapq.modelo.gmodelo.com.mx:8004/sap/bc/srt/rfc/sap/zws_posteo_correcciones/101/zws_posteo_correcciones/zws_posteo_correcciones"));

            _channelFactory.Credentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication()
            {
                CertificateValidationMode = X509CertificateValidationMode.None,
                RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck
            };
            _channelFactory.Credentials.UserName.UserName = "WEBBACKOFF2";
            _channelFactory.Credentials.UserName.Password = "Mitologia.32@&5.0";
            _zws_Posteo_Correcciones = _channelFactory.CreateChannel();
        }

        /// <summary>
        /// Metodo para enviar cancelaciones a sap
        /// </summary>
        /// <param name="documentoOriginalNCFs">documentoOriginalNCFs</param>
        /// <returns></returns>
        public (bool, ZMF_POSTEO_CORRECCIONESResponse1) EnviarAnulacionSap(List<DocumentoOriginalNCF> documentoOriginalNCFs)
        {
            var ZmfPostCorreccion = new ZMF_POSTEO_CORRECCIONES
            {
                T_LOG_DOC_CREADOS = Array.Empty<ZST_COR_CREADAS>(),
                T_LOG_ERRORES = Array.Empty<ZST_LOG_ERRORES>(),
                ZTB_POSTEO = new ZST_DOC_CORRECCIONES[documentoOriginalNCFs.Count()]
            };
            var respuesta = new ZMF_POSTEO_CORRECCIONESResponse1();

            for (int i = 0; i < documentoOriginalNCFs.Count; i++)
            {
                var camposEnvioPost = new ZST_DOC_CORRECCIONES();
                camposEnvioPost.SOCIEDAD = documentoOriginalNCFs[i].IdCompany;
                camposEnvioPost.COD_CLIENTE = documentoOriginalNCFs[i].IdCustumer;
                camposEnvioPost.TIPO_PEDIDO = documentoOriginalNCFs[i].TipoSapCancelacion;
                camposEnvioPost.FECH_DOCUMENTO = DateTime.Now.Date.ToString("yyyy-MM-dd");
                camposEnvioPost.NCF_NUEVO = documentoOriginalNCFs[i].NCFCancelacion;
                camposEnvioPost.NCF_ORIGEN = documentoOriginalNCFs[i].NCF;
                camposEnvioPost.MATERIAL = documentoOriginalNCFs[i].IdProduct;
                camposEnvioPost.UNI_MEDIDA = documentoOriginalNCFs[i].IdUnitMeasureType;
                camposEnvioPost.CANTIDAD = documentoOriginalNCFs[i].Amount.ToString();
                camposEnvioPost.AMOU_GROSS = decimal.Round(documentoOriginalNCFs[i].BrutoTotal, 2);
                camposEnvioPost.DISC_VOL = documentoOriginalNCFs[i].DescuentoAmount.ToString();
                camposEnvioPost.TAX_ITBIS = decimal.Round(documentoOriginalNCFs[i].TaxAmount, 2);
                camposEnvioPost.TAX_ISC = decimal.Round(documentoOriginalNCFs[i].Isc, 2);
                camposEnvioPost.TAX_ISCE = decimal.Round(documentoOriginalNCFs[i].Isce, 2);
                camposEnvioPost.NETO = decimal.Round(documentoOriginalNCFs[i].NetAmount, 2);
                ZmfPostCorreccion.ZTB_POSTEO[i] = camposEnvioPost;
            }
            try
            {
                var postSap = new ZMF_POSTEO_CORRECCIONESRequest(ZmfPostCorreccion);
                respuesta = _zws_Posteo_Correcciones.ZMF_POSTEO_CORRECCIONES(postSap);
                return (true, respuesta);
            }
            catch (Exception)
            {
                _channelFactory.Close();
                return (false, respuesta);
            }
        }

        /// <summary>
        /// Metodo para envio a sap de documneto ajuste
        /// </summary>
        /// <param name="documentoCorreccionNCFs">docuemntos de ajuste para enviar a sap</param>
        /// <returns></returns>
        public (bool, ZMF_POSTEO_CORRECCIONESResponse1) EnviarAjusteSap(List<DocumentoCorreccionNCF> documentoCorreccionNCFs)
        {
            var ZmfPostCorreccion = new ZMF_POSTEO_CORRECCIONES
            {
                T_LOG_DOC_CREADOS = Array.Empty<ZST_COR_CREADAS>(),
                T_LOG_ERRORES = Array.Empty<ZST_LOG_ERRORES>(),
                ZTB_POSTEO = new ZST_DOC_CORRECCIONES[documentoCorreccionNCFs.Count()]
            };
            var respuesta = new ZMF_POSTEO_CORRECCIONESResponse1();

            for (int i = 0; i < documentoCorreccionNCFs.Count; i++)
            {
                var camposEnvioPost = new ZST_DOC_CORRECCIONES();
                camposEnvioPost.SOCIEDAD = documentoCorreccionNCFs[i].IdCompany;
                camposEnvioPost.COD_CLIENTE = documentoCorreccionNCFs[i].IdCustumer;
                camposEnvioPost.TIPO_PEDIDO = documentoCorreccionNCFs[i].TipoSapCorrecion;
                camposEnvioPost.FECH_DOCUMENTO = DateTime.Now.Date.ToString("yyyy-MM-dd");
                camposEnvioPost.NCF_NUEVO = documentoCorreccionNCFs[i].NCFCorreccion;
                camposEnvioPost.NCF_ORIGEN = documentoCorreccionNCFs[i].NCF;
                camposEnvioPost.MATERIAL = documentoCorreccionNCFs[i].IdProduct;
                camposEnvioPost.UNI_MEDIDA = documentoCorreccionNCFs[i].IdUnitMeasureType;
                camposEnvioPost.CANTIDAD = documentoCorreccionNCFs[i].Amount.ToString();
                camposEnvioPost.AMOU_GROSS = decimal.Round(documentoCorreccionNCFs[i].BrutoTotal, 2);
                camposEnvioPost.DISC_VOL = documentoCorreccionNCFs[i].DescuentoAmount.ToString();
                camposEnvioPost.TAX_ITBIS = decimal.Round(documentoCorreccionNCFs[i].TaxAmount, 2);
                camposEnvioPost.TAX_ISC = decimal.Round(documentoCorreccionNCFs[i].Isc, 2);
                camposEnvioPost.TAX_ISCE = decimal.Round(documentoCorreccionNCFs[i].Isce, 2);
                camposEnvioPost.NETO = decimal.Round(documentoCorreccionNCFs[i].NetAmount, 2);
                ZmfPostCorreccion.ZTB_POSTEO[i] = camposEnvioPost;
            }
            try
            {
                var postSap = new ZMF_POSTEO_CORRECCIONESRequest(ZmfPostCorreccion);
                respuesta = _zws_Posteo_Correcciones.ZMF_POSTEO_CORRECCIONES(postSap);
                return (true, respuesta);
            }
            catch (Exception)
            {
                _channelFactory.Close();
                return (false, respuesta);
            }
        }
    }
}
