using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mag.VisualizationLocation.Adapter.DataContext
{
    public interface IMtbContext
    {
        Place[] GetPlaces(Filter filter);
    }
    public class MtbContext:IMtbContext
    {
        private readonly ILogger _logger;
        private readonly Source _source;

        public MtbContext(IOptions<Source> options, ILogger<MtbContext> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if(options==null)
                throw new ArgumentNullException(nameof(options));
            _source = options.Value;
        }
        public Place[] GetPlaces(Filter filter)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder
            {
                ApplicationName = _source.ApplicationName,
                UserID = filter.User,
                Password = filter.Password,
                DataSource = _source.DataSource,
                InitialCatalog = _source.InitialCatalog
            };

            var query =
                "SELECT pl.mcc MCC, pl.mnc MNC,pl.lac Lac, pl.cl Cid, pl.in_date InDate, celdic.coordinate_x Latitude, celdic.coordinate_y Longitude " +
                " FROM ot_place pl " +
                " join dt_mr_TasksGrants tg on tg.task_auto_id = pl.task_auto_id " +
                $" join dt_Tasks ts on tg.oper_task_id = ts.task_id and ts.task_guid = '{filter.OtmGuid}' " +
                " left outer join lc_CellDict celdic on celdic.code = pl.cl " +
                $" where pl.in_date >= '{filter.From:s}' and pl.in_date <= '{filter.To:s}' and pl.in_date >= tg.auto_start and pl.in_date <= tg.auto_end";

            var places = new Place[0];
            try
            {
                using (var connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
                {
                    places = connection.Query<Place>(query, new
                    {
                        from = filter.From.ToString("yyyy-MM-dd hh:mm:ss"),
                        to = filter.To.ToString("yyyy-MM-dd hh:mm:ss"),
                        taskGuid = filter.OtmGuid
                    }).ToArray();

                    _logger.LogInformation($"Получено местоположений {places.Length}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка получения местоположений из МТБ", ex);
            }

            return places;
        }
    }
}
