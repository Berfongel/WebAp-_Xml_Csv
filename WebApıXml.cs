using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebApıXml : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult ApıXml()
        {

            /*veritabanında bağlantı*/
            SqlConnection con = new SqlConnection(@"Data Source=KALITE\SQLEXPRESS;Initial Catalog=WebApı;Integrated Security=True");
            
                con.Open();
            /*veritabanındaki tabloyu listeliyor*/
            SqlCommand com = new SqlCommand("SELECT CityName,CityCode,DistrictName,ZipCode FROM DATA FOR XML PATH('DATA')", con);

            SqlDataAdapter sdp = new SqlDataAdapter(com);
                    
                        DataSet ds = new DataSet();
                        sdp.Fill(ds);
                        /*xml uzantısı*/
                        string csvData = XmlWriteMode(ds.Tables[0]);
                        //StreamReader sr = new StreamReader(csvData,Encoding.GetEncoding(""));
                        var fileBytes = Encoding.UTF8.GetBytes(csvData);
                        /*dosyayı hangi uzantıda indirmemiz gerektiği*/
                        return File(fileBytes, "application/xml", "Data.xml");               
       
    }
        private string XmlWriteMode(DataTable dataTable)
        {

            StringBuilder csvBuilder = new StringBuilder();
            IEnumerable<string> columNames = (IEnumerable<string>)dataTable.Columns.Cast<DataColumn>()
                .Select(x => x.ColumnName);
            /*kolonlar adları yazılıyor*/
            csvBuilder.AppendLine(string.Join(" ", columNames));
            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<string> file = row.ItemArray.Select
                    (x => string.Concat("  ", x.ToString().Replace("  ", "   "), " "));
                csvBuilder.AppendLine(string.Join(' ', file));

            }
            return csvBuilder.ToString();
        }
    }
}
