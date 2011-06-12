﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace HandBrake.Interop
{
	public static class HBFunctions
	{
		[DllImport("hb.dll", EntryPoint = "hb_register_logger", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_register_logger(LoggingCallback callback);

		[DllImport("hb.dll", EntryPoint = "hb_register_error_handler", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_register_error_handler(LoggingCallback callback);

		/// Return Type: hb_handle_t*
		///verbose: int
		///update_check: int
		[DllImport("hb.dll", EntryPoint = "hb_init", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr hb_init(int verbose, int update_check);


		/// Return Type: hb_handle_t*
		///verbose: int
		///update_check: int
		[DllImport("hb.dll", EntryPoint = "hb_init_dl", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr hb_init_dl(int verbose, int update_check);


		/// Return Type: char*
		///param0: hb_handle_t*
		[DllImport("hb.dll", EntryPoint = "hb_get_version", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr hb_get_version(ref hb_handle_s param0);


		/// Return Type: int
		///param0: hb_handle_t*
		[DllImport("hb.dll", EntryPoint = "hb_get_build", CallingConvention = CallingConvention.Cdecl)]
		public static extern int hb_get_build(ref hb_handle_s param0);


		/// Return Type: int
		///h: hb_handle_t*
		///version: char**
		[DllImport("hb.dll", EntryPoint = "hb_check_update", CallingConvention = CallingConvention.Cdecl)]
		public static extern int hb_check_update(ref hb_handle_s h, ref IntPtr version);


		/// Return Type: void
		///param0: hb_handle_t*
		///param1: int
		[DllImport("hb.dll", EntryPoint = "hb_set_cpu_count", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_set_cpu_count(ref hb_handle_s param0, int param1);


		/// Return Type: char*
		///path: char*
		[DllImport("hb.dll", EntryPoint = "hb_dvd_name", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr hb_dvd_name(IntPtr path);


		/// Return Type: void
		///enable: int
		[DllImport("hb.dll", EntryPoint = "hb_dvd_set_dvdnav", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_dvd_set_dvdnav(int enable);


		/// Return Type: void
		///param0: hb_handle_t*
		///path: char*
		///title_index: int
		///preview_count: int
		///store_previews: int
		[DllImport("hb.dll", EntryPoint = "hb_scan", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_scan(IntPtr hbHandle, [In] [MarshalAs(UnmanagedType.LPStr)] string path, int title_index, int preview_count, int store_previews, ulong min_duration);

		[DllImport("hb.dll", EntryPoint = "hb_scan_stop", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_scan_stop(IntPtr hbHandle);

		/// Return Type: hb_list_t*
		///param0: hb_handle_t*
		[DllImport("hb.dll", EntryPoint = "hb_get_titles", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr hb_get_titles(IntPtr hbHandle);


		/// Return Type: int
		///buf: hb_buffer_t*
		///width: int
		///height: int
		///color_equal: int
		///color_diff: int
		///threshold: int
		///prog_equal: int
		///prog_diff: int
		///prog_threshold: int
		[DllImport("hb.dll", EntryPoint = "hb_detect_comb", CallingConvention = CallingConvention.Cdecl)]
		public static extern int hb_detect_comb(ref hb_buffer_s buf, int width, int height, int color_equal, int color_diff, int threshold, int prog_equal, int prog_diff, int prog_threshold);

		[DllImport("hb.dll", EntryPoint = "hb_get_preview_by_index", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_get_preview_by_index(IntPtr hbHandle, int title_index, int picture, IntPtr buffer);

		/// Return Type: void
		///param0: hb_handle_t*
		///param1: hb_title_t*
		///param2: int
		///param3: uint8_t*
		[DllImport("hb.dll", EntryPoint = "hb_get_preview", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_get_preview(IntPtr hbHandle, ref hb_title_s title, int preview, IntPtr buffer);


		/// Return Type: void
		///param0: hb_job_t*
		///ratio: double
		///pixels: int
		[DllImport("hb.dll", EntryPoint = "hb_set_size", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_set_size(ref hb_job_s param0, double ratio, int pixels);

		[DllImport("hb.dll", EntryPoint = "hb_set_anamorphic_size_by_index", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_set_anamorphic_size_by_index(IntPtr hbHandle, int title_index, ref int output_width, ref int output_height, ref int output_par_width, ref int output_par_height);

		/// Return Type: void
		///param0: hb_job_t*
		///output_width: int*
		///output_height: int*
		///output_par_width: int*
		///output_par_height: int*
		[DllImport("hb.dll", EntryPoint = "hb_set_anamorphic_size", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_set_anamorphic_size(ref hb_job_s job, ref int output_width, ref int output_height, ref int output_par_width, ref int output_par_height);


		/// Return Type: int
		///param0: hb_handle_t*
		[DllImport("hb.dll", EntryPoint = "hb_count", CallingConvention = CallingConvention.Cdecl)]
		public static extern int hb_count(IntPtr hbHandle);


		/// Return Type: hb_job_t*
		///param0: hb_handle_t*
		///param1: int
		[DllImport("hb.dll", EntryPoint = "hb_job", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr hb_job(IntPtr hbHandle, int jobIndex);

		[DllImport("hb.dll", EntryPoint = "hb_set_chapter_name", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_set_chapter_name(IntPtr hbHandle, int title_index, int chapter_index, [In] [MarshalAs(UnmanagedType.LPStr)] string chapter_name);

		[DllImport("hb.dll", EntryPoint = "hb_set_job", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_set_job(IntPtr hbHandle, int title_index, ref hb_job_s job);

		/// Return Type: void
		///param0: hb_handle_t*
		///param1: hb_job_t*
		[DllImport("hb.dll", EntryPoint = "hb_add", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_add(IntPtr hbHandle, ref hb_job_s job);


		/// Return Type: void
		///param0: hb_handle_t*
		///param1: hb_job_t*
		[DllImport("hb.dll", EntryPoint = "hb_rem", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_rem(IntPtr hbHandle, IntPtr job);


		/// Return Type: void
		///param0: hb_handle_t*
		[DllImport("hb.dll", EntryPoint = "hb_start", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_start(IntPtr hbHandle);


		/// Return Type: void
		///param0: hb_handle_t*
		[DllImport("hb.dll", EntryPoint = "hb_pause", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_pause(IntPtr hbHandle);


		/// Return Type: void
		///param0: hb_handle_t*
		[DllImport("hb.dll", EntryPoint = "hb_resume", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_resume(IntPtr hbHandle);


		/// Return Type: void
		///param0: hb_handle_t*
		[DllImport("hb.dll", EntryPoint = "hb_stop", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_stop(IntPtr hbHandle);

		[DllImport("hb.dll", EntryPoint = "hb_get_filter_object", CallingConvention = CallingConvention.Cdecl)]
		//public static extern IntPtr hb_get_filter_object(int filter_id, [In] [MarshalAs(UnmanagedType.LPStr)] string settings);
		public static extern IntPtr hb_get_filter_object(int filter_id, IntPtr settings);

		/// Return Type: void
		///param0: hb_handle_t*
		///param1: hb_state_t*
		[DllImport("hb.dll", EntryPoint = "hb_get_state", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_get_state(IntPtr hbHandle, ref hb_state_s state);


		/// Return Type: void
		///param0: hb_handle_t*
		///param1: hb_state_t*
		[DllImport("hb.dll", EntryPoint = "hb_get_state2", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_get_state2(ref hb_handle_s param0, ref hb_state_s param1);


		/// Return Type: int
		///param0: hb_handle_t*
		[DllImport("hb.dll", EntryPoint = "hb_get_scancount", CallingConvention = CallingConvention.Cdecl)]
		public static extern int hb_get_scancount(ref hb_handle_s param0);


		/// Return Type: void
		///param0: hb_handle_t**
		[DllImport("hb.dll", EntryPoint = "hb_close", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_close(IntPtr hbHandle);

		[DllImport("hb.dll", EntryPoint = "hb_global_close", CallingConvention = CallingConvention.Cdecl)]
		public static extern void hb_global_close();

		[DllImport("hb.dll", EntryPoint = "hb_subtitle_add", CallingConvention = CallingConvention.Cdecl)]
		public static extern int hb_subtitle_add(ref hb_job_s job, ref hb_subtitle_config_s subtitleConfig, int track);

		[DllImport("hb.dll", EntryPoint = "hb_srt_add", CallingConvention = CallingConvention.Cdecl)]
		public static extern int hb_srt_add(ref hb_job_s job, ref hb_subtitle_config_s subtitleConfig, string lang);

		[DllImport("hb.dll", EntryPoint = "hb_get_default_mixdown", CallingConvention = CallingConvention.Cdecl)]
		public static extern int hb_get_default_mixdown(uint codec, int layout);

		[DllImport("hb.dll", EntryPoint = "hb_get_best_mixdown", CallingConvention = CallingConvention.Cdecl)]
		public static extern int hb_get_best_mixdown(uint codec, int layout, int mixdown);

		[DllImport("hb.dll", EntryPoint = "hb_get_best_audio_bitrate", CallingConvention = CallingConvention.Cdecl)]
		public static extern int hb_get_best_audio_bitrate(uint codec, int bitrate, int samplerate, int mixdown);

		[DllImport("hb.dll", EntryPoint = "hb_get_audio_bitrate_limits", CallingConvention = CallingConvention.Cdecl)]
		public static extern int hb_get_audio_bitrate_limits(uint codec, int samplerate, int mixdown, ref int low, ref int high);
	}
}