using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Extensions.Options;

namespace Mag.VisualizationLocation.Adapter.Client.Tests.TestContext
{
    public interface ITestMtbContext
    {
        AutoTaskDto AddModel();
        OtmTaskDto AddModel(OtmTaskDto model);
        CellDictDto[] AddModels(CellDictDto[] models);
        PlaceDto[] AddModels(PlaceDto[] models);
        TaskGrantDto AddModel(TaskGrantDto model);
        void ClearTables();
    }

    public class TestMtbContext : ITestMtbContext
    {
        private readonly string _connectionString;

        public TestMtbContext(IOptions<TestDbConnection> options)
        {
            _connectionString = options.Value.ConnectionString;
        }
        public AutoTaskDto AddModel()
        {
            var query = "INSERT INTO dt_TasksAuto(reg_number, source_id, file_path, device_id, channel, control_phone, active_flag) "+
            "VALUES(@regNumber, 100, '\\\\magtalks\\2019\\100.mag', 100, 100, '2332020', 1) "+
            "select task_auto_id Id, reg_number RegNumber, task_auto_guid TaskAutoGuid from dt_TasksAuto "+
            "where reg_number = @regNumber";

            using (var connection = new SqlConnection(_connectionString))
            {
                var id = connection.ExecuteScalar<int>("select Max(task_auto_id) from dt_TasksAuto") ;
                var autoTask = connection.Query<AutoTaskDto>(query, new { regNumber = "regnumber-" + id+1 }).ToArray();
                return autoTask.First();
            }
        }

        public OtmTaskDto AddModel(OtmTaskDto model)
        {
            var query =
                "INSERT INTO dt_Tasks (obj_shifr, obj_name, category_id, ctrl_allowed, fax_processing, task_srok, data_type, security_id, task_type, RestrictExport) " +
                "VALUES(@objShifr, @objName, 0, 0, 0, 30, 0, 1, 0, 0)"+
                "select task_id Id, obj_shifr  objShifr, task_guid TaskGuid from dt_Tasks where obj_shifr = @objShifr";
            using (var connection = new SqlConnection(_connectionString))
            {
                var lastId = connection.ExecuteScalar<int>("select MAX(task_id) from dt_Tasks");
                var otm = connection.QueryFirst<OtmTaskDto>(query,
                    new {objShifr = "objShifr-" + lastId + 1, objName = "objName-" + lastId + 1});
                return otm;
            }
        }
        public PlaceDto[] AddModels(PlaceDto[] models)
        {
            var builder = new StringBuilder();
            var inserts = models.Select(pl =>
                "INSERT INTO ot_place (task_auto_id, mcc, mnc, lac, cl, in_date, state,  arc_flag, dev_state, data_label, time_last_session, talk_id) " + $"VALUES({pl.TaskAutoId},{pl.MCC}, {pl.MNC}, {pl.Lac}, {pl.Cid},'{pl.InDate:s}',0, 0, 0, 0, 0,null)");
            foreach (var sql in inserts)
            {
                builder.AppendLine(sql);
            }
            var select = $"select top({models.Length}) place_id Id, task_auto_id TaskAutoId, in_date InDate, cl Cid, mcc MCC, mnc MNC, lac Lac  from ot_place order by place_id desc";
            builder.AppendLine(select);

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Query<PlaceDto>(builder.ToString()).ToArray();
                return result;
            }
        }
        public TaskGrantDto AddModel(TaskGrantDto model)
        {
            var query = "INSERT INTO dt_mr_TasksGrants(oper_task_id, task_auto_id, auto_start, auto_end) " +
                        "VALUES(@oper_task_id, @task_auto_id, @auto_start, @auto_end) \n" +

                        "select task_grants_id Id, oper_task_id OtmId, task_auto_id AutoTaskId,auto_start AutoStart, auto_end AutoEnd from dt_mr_TasksGrants " +
                        "where oper_task_id = @oper_task_id and task_auto_id = @task_auto_id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Query<TaskGrantDto>(query, new
                {
                    oper_task_id = model.OtmId,
                    task_auto_id = model.AutoTaskId,
                    auto_start = model.AutoStart,
                    auto_end = model.AutoEnd
                }).First();

                return result;
            }
        }

        public CellDictDto[] AddModels(CellDictDto[] models)
        {
            throw new NotImplementedException();
        }

        public void ClearTables()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("delete from ot_place delete from dt_mr_TasksGrants delete from dt_Tasks delete from dt_TasksAuto");
            }
        }
    }
}
