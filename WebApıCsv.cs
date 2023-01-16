using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebApıCsv : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult ApıCsv()        
        {

            /*veritabanında bağlantı*/
            SqlConnection con = new SqlConnection(@"Data Source=KALITE\SQLEXPRESS;Initial Catalog=WebApı;Integrated Security=True");
            
                con.Open();
            /*veritabanındaki tabloyu listeliyor*/
            SqlCommand com = new SqlCommand("select CityName,CityCode,DistrictName,ZipCode from dbo.Data", con);
                
                 SqlDataAdapter sdp = new SqlDataAdapter(com) ; 
                    
                        DataSet ds = new DataSet();                        
                        sdp.Fill(ds);
                        string csvData = TransformTableToCsv(ds.Tables[0]);
                        //StreamReader sr = new StreamReader(csvData,Encoding.GetEncoding(""));
                        var fileBytes = Encoding.UTF8.GetBytes(csvData);
                        /*dosyayı hangi uzantıda indirmemiz gerektiği*/
                        return File(fileBytes,"text/csv","Data.csv");
            
        }

        private string TransformTableToCsv(DataTable dataTable)
        {
            StringBuilder csvBuilder =new StringBuilder();
            IEnumerable<string> columNames = (IEnumerable<string>)dataTable.Columns.Cast<DataColumn>()
                .Select(x => x.ColumnName);
            /*kolonlar arasına virgül bırakıyor*/
            csvBuilder.AppendLine(string.Join(" , ",columNames));
            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<string> file = row.ItemArray.Select 
                    (x => string.Concat("  ", x.ToString().Replace(" , " , " ,  "), " ,"));
                csvBuilder.AppendLine(string.Join(' ', file)); 

            }
            return csvBuilder.ToString();
        }
    }
}
