/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package pe.edu.pucp.creditomovil.services;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;

import java.util.ArrayList;
import java.util.List;
import java.util.Date;

import pe.edu.pucp.creditomovil.getsclientes.dao.CreditoDAO;
import pe.edu.pucp.creditomovil.getsclientes.mysql.CreditoMySQL;
import pe.edu.pucp.creditomovil.model.Credito;

@WebService(serviceName = "CreditoWS", targetNamespace
        = "https://services.creditomovil.pucp.edu.pe")
public class CreditoWS {

    private CreditoDAO daoCredito = new CreditoMySQL();

    @WebMethod(operationName = "insertarCredito")
    public boolean insertarCredito(@WebParam(name = "credito") Credito credito, 
            @WebParam(name = "codigoCliente") int codigoCliente) {
        boolean res = false;
        try{
            res = daoCredito.insertar(credito,codigoCliente);
        }catch(Exception ex){
            System.out.println(ex.getMessage());
        }
        return res;
    }
    
    @WebMethod(operationName = "modificarCredito")
    public boolean modificarCredito(@WebParam(name = "credito") Credito credito) {
        boolean res = false;
        try{
            res = daoCredito.modificar(credito);
        }catch(Exception ex){
            System.out.println(ex.getMessage());
        }
        return res;
    }
    
    @WebMethod(operationName = "eliminarCredito")
    public void eliminarCredito(@WebParam(name = "credito") int id) {
        try{
            daoCredito.eliminar(id);
        }catch(Exception ex){
            System.out.println(ex.getMessage());
        }
    }
    
    @WebMethod(operationName = "obtenerPorIDCredito")
    public Credito obtenerPorIDCredito(@WebParam(name = "credito") int id) {
        Credito credito = null;
        try{
            credito = daoCredito.obtenerPorId(id);
        }catch(Exception ex){
            System.out.println(ex.getMessage());
        }
        return credito;
    }
    
    @WebMethod(operationName = "listarCreditosClientePorFecha")
    public List<Credito> listarCreditosClientePorFecha(@WebParam(name = "idcli") int idcli,
            @WebParam(name = "fechaini") Date fechaini, @WebParam(name = "fechafin") Date fechafin,
            @WebParam(name = "estado") String estado) {
        List<Credito> creditos = null;
        try{
            creditos = daoCredito.listarTodosFiltros(idcli, fechaini, fechafin, estado);
        }catch(Exception ex){
            System.out.println(ex.getMessage());
        }
        return creditos;
    }
    
    @WebMethod(operationName = "listarCreditosPendXCliente")
    public List<Credito> listarCreditosPendXCliente(@WebParam(name = "codCliente") int codCliente) {
        List<Credito> creditos = null;
        try{
            creditos = daoCredito.listarCredPendPorCliente(codCliente);
        }catch(Exception ex){
            System.out.println(ex.getMessage());
        }
        return creditos;
    }
    
    @WebMethod(operationName = "listarTodosCreditos")
    public List<Credito> listarTodosCreditos() {
        List<Credito> creditos = null;
        try{
            creditos = daoCredito.listarTodos();
        }catch(Exception ex){
            System.out.println(ex.getMessage());
        }
        return creditos;
    }
    
}
