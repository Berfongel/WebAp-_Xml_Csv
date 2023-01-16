using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebApıJson : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult ApıJson()        
        {

            /*veritabanında bağlantı*/
            SqlConnection con = new SqlConnection(@"Data Source=KALITE\SQLEXPRESS;Initial Catalog=WebApı;Integrated Security=True");
            
                con.Open();
            /*veritabanındaki tabloyu listeliyor*/
            /*veritabanındaki tabloyu json olarak ggörmek için gerekli olan sorgu*/
            SqlCommand com = new SqlCommand("select CityName,CityCode,DistrictName,ZipCode from dbo.Data for json path", con);
                
                 SqlDataAdapter sdp = new SqlDataAdapter(com) ; 
                    
                        DataSet ds = new DataSet();                        
                        sdp.Fill(ds);
                        string jsonData = Utf8JsonWriter((ds.Tables[0]));
                        //StreamReader sr = new StreamReader(csvData,Encoding.GetEncoding(""));
                        var fileBytes = Encoding.UTF8.GetBytes(jsonData);
                        /*dosyayı hangi uzantıda indirmemiz gerektiği*/
                        return File(fileBytes,"application/json","Data.json");
            
        }

        private string Utf8JsonWriter(DataTable dataTable)
        {
            StringBuilder jsonBuilder =new StringBuilder();
            IEnumerable<string> columNames = (IEnumerable<string>)dataTable.Columns.Cast<DataColumn>()
                .Select(x => x.ColumnName);
            /*kolonlar arasına virgül bırakıyor*/
            jsonBuilder.AppendLine(string.Join(" , ",columNames));
            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<string> file = row.ItemArray.Select 
                    (x => string.Concat("  ", x.ToString().Replace(" , " , " ,  "), " ,"));
                jsonBuilder.AppendLine(string.Join(' ', file)); 

            }
            return jsonBuilder.ToString();
        }

       
    }
}
