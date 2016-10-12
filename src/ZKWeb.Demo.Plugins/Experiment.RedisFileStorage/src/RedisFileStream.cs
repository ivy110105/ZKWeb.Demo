﻿using System;
using System.IO;

namespace ZKWeb.Demo.Plugins.Experiment.RedisFileStorage.src {
	/// <summary>
	/// Redis的文件流
	/// </summary>
	public class RedisFileStream : MemoryStream {
		/// <summary>
		/// 文件对象
		/// </summary>
		private RedisFileEntry Entry { get; set; } 
		/// <summary>
		/// 文件内容
		/// </summary>
		private RedisFileBody Body { get; set; }
		/// <summary>
		/// 是否已修改过
		/// </summary>
		private bool FileChanged { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public RedisFileStream(RedisFileEntry entry, RedisFileBody body) {
			Entry = entry;
			Body = body;
			var contents = body.GetContents();
			Write(contents, 0, contents.Length);
			Seek(0, SeekOrigin.Begin);
		}

		/// <summary>
		/// 写入数据
		/// </summary>
		public override void Write(byte[] buffer, int offset, int count) {
			FileChanged = true;
			base.Write(buffer, offset, count);
		}

		/// <summary>
		/// 写入数据
		/// </summary>
		public override void WriteByte(byte value) {
			FileChanged = true;
			base.WriteByte(value);
		}

		/// <summary>
		/// 关闭文件时保存到Redis
		/// </summary>
		public override void Close() {
			if (FileChanged) {
				Seek(0, SeekOrigin.Begin);
				var contents = new byte[Length];
				Read(contents, 0, contents.Length);
				Body.SetContents(contents);
				Body.LastAccessTime = DateTime.UtcNow;
				Body.LastWriteTime = DateTime.UtcNow;
				Entry.UpdateBody(Body);
				FileChanged = false;
			}
			base.Close();
		}
	}
}
