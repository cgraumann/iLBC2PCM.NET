	#include <math.h>
	#include <stdlib.h>
	#include <stdio.h>
	#include <string.h>
	#include "iLBC_define.h"
	#include "iLBC_decode.h"
#include "iLBCConverter.h"


	void TestDLL()
	{
		printf("DLL iLBCConverter properly included.");
	}

	short decodeBlock(short decodedData[], short encodedData[], short mode)
	{
		iLBC_Dec_Inst_t decoder;
		initDecode(&decoder, mode, 1);
		int k;
       float decblock[BLOCKL_MAX], dtmp;

       /* check if mode is valid */

       if (mode<0 || mode>1) {
           printf("\nERROR - Wrong mode - 0, 1 allowed\n"); exit(3);}

       /* do actual decoding of block */
	   short* enc = &encodedData[0];

       iLBC_decode(decblock, (unsigned char *)enc,
           &decoder, mode);

       /* convert to short */

      for (k=0; k<decoder.blockl; k++){
           dtmp=decblock[k];

           if (dtmp<MIN_SAMPLE)
               dtmp=MIN_SAMPLE;
           else if (dtmp>MAX_SAMPLE)
               dtmp=MAX_SAMPLE;
           decodedData[k] = (short) dtmp;
       }

       return (decoder.blockl);
	}


