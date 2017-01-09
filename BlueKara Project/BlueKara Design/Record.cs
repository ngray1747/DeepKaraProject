using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueKara_Design
{
    public class RecordYourVoice
    {
        private static WaveIn sourcestream;
        private static WaveFileWriter wavewriter;

        public static void RecordVoice(string URLfile)
        {
            int devicenumber = 0;
            sourcestream = new WaveIn();
            sourcestream.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(devicenumber).Channels);

            sourcestream.DataAvailable += new EventHandler<WaveInEventArgs>(sourcestream_DataAvailbale);
            wavewriter = new WaveFileWriter(URLfile, sourcestream.WaveFormat);
            sourcestream.StartRecording();


        }
        public static void StopRecord()
        {
            if (sourcestream != null)
            {


                sourcestream.StopRecording();
                sourcestream.Dispose();

                wavewriter.Dispose();

            }
        }
        private static void sourcestream_DataAvailbale(object sender, WaveInEventArgs e)
        {
            if (wavewriter == null) return;

            wavewriter.WriteData(e.Buffer, 0, e.BytesRecorded);
            //wavewriter.Flush();

        }
    }
    public class RecordAudioSystem
    {
        private static LameMP3FileWriter wri;

        private static WasapiLoopbackCapture waveIn;



        public static void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // write recorded data to MP3 writer
            if (wri != null)
                wri.Write(e.Buffer, 0, e.BytesRecorded);
        }
        public static void RecordSystem(string URLFileOut)
        {
            // Start recording from loopback
            waveIn = new WasapiLoopbackCapture();
            waveIn.DataAvailable += waveIn_DataAvailable;

            // Setup MP3 writer to output at 128kbit/sec 
            wri = new LameMP3FileWriter(URLFileOut, waveIn.WaveFormat, 128);
            waveIn.StartRecording();


        }
        public static void StopRecord()
        {
            if (waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                wri.Dispose();
            }
        }
    }
}
public class ConvertFile
{
    public static void MixWavFiles(string[] inputFiles, string outFileName)
    {
        int count = inputFiles.GetLength(0);
        WaveMixerStream32 mixer = new WaveMixerStream32();
        WaveFileReader[] reader = new WaveFileReader[count];
        WaveChannel32[] channelSteam = new WaveChannel32[count];
        mixer.AutoStop = true;

        for (int i = 0; i < count; i++)
        {
            reader[i] = new WaveFileReader(inputFiles[i]);
            channelSteam[i] = new WaveChannel32(reader[i]);
            mixer.AddInputStream(channelSteam[i]);
        }
        mixer.Position = 0;
        WaveFileWriter.CreateWaveFile(outFileName, mixer);

    }

    public static void NAudioMp3ToWav(string mp3File, string outwavFile)
    {
        using (Mp3FileReader reader = new Mp3FileReader(mp3File))
        {
            using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
            {
                WaveFileWriter.CreateWaveFile(outwavFile, pcmStream);

            }
        }
    }
}
