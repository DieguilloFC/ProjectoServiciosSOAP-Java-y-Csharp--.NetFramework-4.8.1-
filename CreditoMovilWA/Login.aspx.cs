﻿using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using CreditoMovilWA.CreditoMovil;
using System.Web.Security;
using System.Web;
using System.Text;

namespace CreditoMovilWA
{
    public partial class Login : System.Web.UI.Page
    {
        private ClienteWSClient daoCliente = new ClienteWSClient();
        private SupervisorWSClient daoSupervisor = new SupervisorWSClient();
        private AdministradorWSClient daoAdministrador = new AdministradorWSClient();
        protected static cliente[] todoClientes = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is Usuario masterPage)
            {
                masterPage.MostrarHeader = false; // Oculta el header en esta página
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {

            string tipoDocumento = ddlTipoDocumento.SelectedValue;
            string numDocumentoIdentidad = txtDocumento.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(tipoDocumento) || string.IsNullOrEmpty(numDocumentoIdentidad) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Por favor, ingrese todos sus datos.";
                lblError.Visible = true;
                return; // Detener la ejecución si faltan datos
            }

            if (tipoDocumento != null && numDocumentoIdentidad != null && password != null)
            {

                cliente cli = daoCliente.obtenerPorDocIdenCliente(numDocumentoIdentidad, tipoDocumento);
                supervisor sup = daoSupervisor.obtenerPorDocIdenSup(numDocumentoIdentidad, tipoDocumento);
                administrador admin = daoAdministrador.obtenerPorDocIdenAdmin(numDocumentoIdentidad, tipoDocumento);

                Session["Cliente"] = cli;
                Session["Supervisor"] = sup;
                Session["Administrador"] = admin;
                if (cli != null)
                {
                    if (VerificarContraseña(password, cli.salt, cli.contrasenha))
                    {
                        FormsAuthenticationTicket tkt;
                        string cookiestr;
                        HttpCookie ck;
                        tkt = new FormsAuthenticationTicket(1, cli.codigoCliente.ToString(), DateTime.Now,
                        DateTime.Now.AddMinutes(30), true, cli.nombre + " " + cli.apPaterno + " " + cli.apMaterno);
                        cookiestr = FormsAuthentication.Encrypt(tkt);
                        ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
                        ck.Expires = tkt.Expiration; //esto genera que la cookie se quede guardada
                        ck.Path = FormsAuthentication.FormsCookiePath;
                        Response.Cookies.Add(ck);

                        if (cli.tipoCliente.ToUpper().Equals("NUEVO"))
                        {
                            Response.Redirect("DatosEvaluacionCliente.aspx");
                        }
                        else
                        {
                            Response.Redirect("MainCliente.aspx");
                        }
                    }
                    else
                    {
                        lblError.Text = "Usuario o contraseña incorrectos.";
                        lblError.Visible = true;
                        return;
                    }
                }
                else if (sup != null)
                {
                    if (VerificarContraseña(password, sup.salt, sup.contrasenha))
                    {
                        FormsAuthenticationTicket tkt;
                        string cookiestr;
                        HttpCookie ck;
                        tkt = new FormsAuthenticationTicket(1, sup.codigoEv.ToString(), DateTime.Now,
                        DateTime.Now.AddMinutes(30), true, sup.nombre + " " + sup.apPaterno + " " + sup.apMaterno);
                        cookiestr = FormsAuthentication.Encrypt(tkt);
                        ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
                        ck.Expires = tkt.Expiration; //esto genera que la cookie se quede guardada
                        ck.Path = FormsAuthentication.FormsCookiePath;
                        Response.Cookies.Add(ck);

                        string strRedirect;
                        strRedirect = Request["ReturnUrl"];
                        if (strRedirect == null)
                            strRedirect = "MainSupervisor.aspx";
                        Response.Redirect(strRedirect, true);
                    }
                    else
                    {
                        lblError.Text = "Usuario o contraseña incorrectos.";
                        lblError.Visible = true;
                        return;
                    }
                }
                else if (admin != null)
                {
                    if (admin.contrasenha == password)
                    {
                        FormsAuthenticationTicket tkt;
                        string cookiestr;
                        HttpCookie ck;
                        tkt = new FormsAuthenticationTicket(1, admin.codigoAdm.ToString(), DateTime.Now,
                        DateTime.Now.AddMinutes(30), true, admin.nombre + " " + admin.apPaterno + " " + admin.apMaterno);
                        cookiestr = FormsAuthentication.Encrypt(tkt);
                        ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
                        ck.Expires = tkt.Expiration; //esto genera que la cookie se quede guardada
                        ck.Path = FormsAuthentication.FormsCookiePath;
                        Response.Cookies.Add(ck);

                        string strRedirect;
                        strRedirect = Request["ReturnUrl"];
                        if (strRedirect == null)
                            strRedirect = "MainAdmin.aspx";
                        Response.Redirect(strRedirect, true);
                    }
                    else
                    {
                        lblError.Text = "Usuario o contraseña incorrectos.";
                        lblError.Visible = true;
                        return;
                    }
                }
                lblError.Text = "Usuario o contraseña incorrectos.";
                lblError.Visible = true;
                return;
            }
            else
            {
                lblError.Text = "Usuario o contraseña incorrectos.";
                lblError.Visible = true;
            }

        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        private bool VerificarContraseña(string contraseñaIngresada, string salAlmacenada, string contraseñaHashAlmacenada)
        {
            // Combinar la contraseña ingresada con el salt almacenado
            string saltedPassword = contraseñaIngresada + salAlmacenada;

            // Recalcular el hash usando SHA256
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                string computedHash = Convert.ToBase64String(hashBytes);

                // Comparar el hash calculado con el hash almacenado
                return computedHash == contraseñaHashAlmacenada;
            }
        }
    }
}