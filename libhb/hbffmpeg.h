/* hbffmpeg.h

   Copyright (c) 2003-2012 HandBrake Team
   This file is part of the HandBrake source code
   Homepage: <http://handbrake.fr/>.
   It may be used under the terms of the GNU General Public License v2.
   For full terms see the file COPYING file or visit http://www.gnu.org/licenses/gpl-2.0.html
 */

#include "libavcodec/avcodec.h"
#include "libavformat/avformat.h"
#include "libavutil/opt.h"
#include "libavutil/mathematics.h"
#include "libavutil/imgutils.h"
#include "libswscale/swscale.h"

#define HB_FFMPEG_THREADS_AUTO (-1) // let hb_avcodec_open decide thread_count

void hb_avcodec_init(void);
int hb_avcodec_open( AVCodecContext *, struct AVCodec *, AVDictionary **av_opts, int thread_count );
int hb_avcodec_close( AVCodecContext * );
int hb_ff_layout_xlat(int64_t ff_layout, int channels);
struct SwsContext* hb_sws_get_context( int srcW, int srcH, 
    enum PixelFormat srcFormat, int dstW, int dstH, 
    enum PixelFormat dstFormat, int flags);
void hb_ff_set_sample_fmt(AVCodecContext *context, AVCodec *codec);
int hb_ff_dts_disable_xch( AVCodecContext *c );
int hb_avpicture_fill( AVPicture *pic, hb_buffer_t *buf );
