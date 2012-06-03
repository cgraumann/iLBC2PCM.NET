using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAFReading
{

    interface CAFChunkContent
    {
        // Interface just for grouping purpose
    }

    enum CAFChunkType : int {
        //Description = "desc", //not needed whilst description chunk being stored directly in class CAFReader
        AudioData,
        PacketTable,
        ChannelLayout,
        MagicCookie,
        Strings,
        Marker,
        Region,
        Instrument,
        Midi,
        Overview,
        Peak,
        EditComments,
        Information,
        UniqueMaterialIdentifier,
        UserDefinedChunk,
        FreeSpace
    }

    struct CAFChunk
    {

        private CAFChunkType chunkType;
        public CAFChunkType ChunkType
        {
            get { return chunkType; }
            set { chunkType = value; }
        }

        private long chunkSize;
        public long ChunkSize
        {
            get { return chunkSize; }
            set { chunkSize = value; }
        }

        private CAFChunkContent content;
        public CAFChunkContent Content
        {
            get { return content; }
            set { content = value; }
        }

        public CAFChunk(CAFChunkType type, long size, CAFChunkContent content)
        {
            this.chunkType = type;
            this.chunkSize = size;
            this.content = content;
        }
    }
}
