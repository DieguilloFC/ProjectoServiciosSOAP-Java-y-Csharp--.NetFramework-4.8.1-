/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Interface.java to edit this template
 */
package pe.edu.pucp.creditomovil.getsclientes.dao;

import java.util.List;
import pe.edu.pucp.creditomovil.model.Evaluacion;

/**
 *
 * @author diego
 */
public interface EvaluacionDAO {
    boolean insertar(Evaluacion evaluacion, int codSup);
    void modificar(Evaluacion evaluacion);
    void eliminar(int idEvaluacion);
    Evaluacion obtenerPorId(int idEvaluacion);
    List<Evaluacion> listarTodosPorSupervisor(int codSup);
    List<Evaluacion> listarPendientesPorSupervisor(int codSup);
    List<Evaluacion> listarRealizadosPorSupervisor(int codSup);
    List<Evaluacion> listarTodos();
}
