using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAFReading
{

    struct CAFChunkHeader
    {
        private UInt32 mChunkType;
        private Int64 mChunkSize;

        public Int64 ChunkSize
        {
            get { return mChunkSize; }
            set { mChunkSize = value; }
        }

        public UInt32 ChunkType
        {
            get { return mChunkType; }
            set { mChunkType = value; }
        }

        public CAFChunkHeader(UInt32 mChunkType, Int64 mChunkSize) { this.mChunkType = mChunkType; this.mChunkSize = mChunkSize; }
        public String chunkTypeAsString()
        {
            return CAFReader.ByteArrayToString(BitConverter.GetBytes(mChunkType));
        }
    };

    struct CAFFileHeader
    {
        private UInt32 mFileType;

        public UInt32 FileType
        {
            get { return mFileType; }
            set { mFileType = value; }
        }
        private UInt16 mFileVersion;

        public UInt16 FileVersion
        {
            get { return mFileVersion; }
            set { mFileVersion = value; }
        }
        private UInt16 mFileFlags;

        public UInt16 FileFlags
        {
            get { return mFileFlags; }
            set { mFileFlags = value; }
        }
        public CAFFileHeader(UInt32 mFileType, UInt16 mFileVersion, UInt16 mFileFlags) { this.mFileType = mFileType; this.mFileVersion = mFileVersion; this.mFileFlags = mFileFlags; }
        public String FileTypeAsString()
        {
            return CAFReader.ByteArrayToString(BitConverter.GetBytes(mFileType));
        }
    };


    struct CAFAudioFormat : CAFChunkContent
    {
        private double mSampleRate;

        public double SampleRate
        {
            get { return mSampleRate; }
            set { mSampleRate = value; }
        }
        private UInt32 mFormatID;

        public UInt32 FormatID
        {
            get { return mFormatID; }
            set { mFormatID = value; }
        }
        private UInt32 mFormatFlags;

        public UInt32 FormatFlags
        {
            get { return mFormatFlags; }
            set { mFormatFlags = value; }
        }
        private UInt32 mBytesPerPacket;

        public UInt32 BytesPerPacket
        {
            get { return mBytesPerPacket; }
            set { mBytesPerPacket = value; }
        }
        private UInt32 mFramesPerPacket;

        public UInt32 FramesPerPacket
        {
            get { return mFramesPerPacket; }
            set { mFramesPerPacket = value; }
        }
        private UInt32 mChannelsPerFrame;

        public UInt32 ChannelsPerFrame
        {
            get { return mChannelsPerFrame; }
            set { mChannelsPerFrame = value; }
        }
        private UInt32 mBitsPerChannel;

        public UInt32 BitsPerChannel
        {
            get { return mBitsPerChannel; }
            set { mBitsPerChannel = value; }
        }

        public String FormatIDAsString()
        {
            return CAFReader.ByteArrayToString(BitConverter.GetBytes(mFormatID));
        }

    };
}
