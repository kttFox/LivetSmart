﻿using System;

namespace LivetSmart.EventListeners {
	/// <summary>
	/// 汎用イベントリスナオブジェクトです。
	/// </summary>
	/// <typeparam name="THandler">イベントハンドラーの型</typeparam>
	public class LivetEventListener<THandler> : IDisposable where THandler : class {
		private THandler? _handler;
		private Action<THandler?>? _add;
		private Action<THandler?>? _remove;
		private bool _disposed;

		private bool _initialized;

		protected LivetEventListener() {

		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="add">h => obj.Event += > h の様な形でイベントの購読を登録するためのAction。hはTHandler型です。</param>
		/// <param name="remove">h => obj.Event -= > h の様な形でイベントの購読を解除するためのAction。hはTHandler型です。</param>
		/// <param name="handler">イベントを受信した際に行いたいアクション</param>
		public LivetEventListener( Action<THandler?> add, Action<THandler?> remove, THandler handler ) {
			Initialize( add, remove, handler );
			_initialized = true;
		}

		protected void Initialize( Action<THandler?> add, Action<THandler?> remove, THandler handler ) {
			if( _initialized ) return;

			_add = add;
			_handler = handler;
			_remove = remove;

			_add( handler );
		}

		protected void ThrowExceptionIfDisposed() {
			if( _disposed ) {
				throw new ObjectDisposedException( "EventListener" );
			}
		}

		/// <summary>
		/// イベントハンドラの登録を解除します。
		/// </summary>
		public void Dispose() {
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing ) {
			if( _disposed ) return;

			if( disposing ) {
				_remove?.Invoke( _handler );
				_add = null;
				_remove = null;
				_handler = null;
			}
			_disposed = true;
		}
	}

}
