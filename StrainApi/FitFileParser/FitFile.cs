using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynastream.Fit;
using Extensions;

namespace FitFileParser
{
    public class FitFile
    {
        public IFormFile FormFile { get; }
        public Guid UserId { get; }

        public FitFile(IFormFile formFile, Guid userId)
        {
            FormFile = formFile;
            UserId = userId;
        }

        public async Task ParseFileAsync()
        {
            var fileStream = new MemoryStream();
            await FormFile.CopyToAsync(fileStream);
            FitDecoder fitDecoder = new FitDecoder(fileStream, Dynastream.Fit.File.Activity);

            // Decode the FIT file
            try
            {
                //Decoding
                fitDecoder.Decode();
            }
            catch (FileTypeException ex)
            {
                //Console.WriteLine("DecodeDemo caught FileTypeException: " + ex.Message);
                return;
            }
            catch (FitException ex)
            {
                //Console.WriteLine("DecodeDemo caught FitException: " + ex.Message);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("DecodeDemo caught Exception: " + ex.Message);
            }
            finally
            {
                fileStream.Close();
            }

            // Check the time zone offset in the Activity message.
            var timezoneOffset = fitDecoder.Messages.Activity.TimezoneOffset();
            //Console.WriteLine($"The timezone offset for this activity file is {timezoneOffset?.TotalHours ?? 0} hours.");

            // Create the Activity Parser and group the messages into individual sessions.
            ActivityParser activityParser = new ActivityParser(fitDecoder.Messages);
            var sessions = activityParser.ParseSessions();

            // Export a CSV file for each Activity Session
            foreach (SessionMessages session in sessions)
            {
                if (session.Records.Count > 0)
                {
                    //This is a CSV of the file
                    var recordsCSV = Export.RecordsToCSV(session);
                }

                if (session.Session.GetSport() == Sport.Swimming && session.Session.GetSubSport() == SubSport.LapSwimming && session.Lengths.Count > 0)
                {
                    //Lengths in a swim session
                    var lengthsCSV = Export.LengthsToCSV(session);
                }
            }
        }
    }
}
