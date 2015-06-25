﻿using System;
using System.Drawing;

namespace ClassicalSharp {
	
	public class LoadingMapScreen : Screen {
		
		readonly Font font;
		public LoadingMapScreen( Game window, string name, string motd ) : base( window ) {
			serverName = name;
			serverMotd = motd;
			font = new Font( "Arial", 14 );
		}
		
		string serverName, serverMotd;
		int progress;
		Texture2D progressBoxTexture;
		TextWidget titleWidget, messageWidget;
		float progX, progY = 100f;
		int	progWidth = 200, progHeight = 40;
		
		public override void Render( double delta ) {			
			GraphicsApi.ClearColour( FastColour.Black );
			titleWidget.Render( delta );
			messageWidget.Render( delta );
			progressBoxTexture.Render( GraphicsApi );
			GraphicsApi.Draw2DQuad( progX, progY, progWidth * progress / 100f, progHeight, FastColour.White );
		}
		
		public override void Init() {
			titleWidget = TextWidget.Create( Window, 0, 30, serverName, Docking.Centre, Docking.LeftOrTop, font );
			messageWidget = TextWidget.Create( Window, 0, 60, serverMotd, Docking.Centre, Docking.LeftOrTop, font );
			progX = Window.Width / 2f - progWidth / 2f;
			
			Size size = new Size( progWidth, progHeight );
			using( Bitmap bmp = Utils2D.CreatePow2Bitmap( size ) ) {
				using( Graphics g = Graphics.FromImage( bmp ) ) {
					Utils2D.DrawRectBounds( g, Color.White, 5f, 0, 0, progWidth, progHeight );
				}
				progressBoxTexture = Utils2D.Make2DTexture( GraphicsApi, bmp, size, (int)progX, (int)progY );
			}
			Window.MapLoading += MapLoading;
		}

		void MapLoading( object sender, MapLoadingEventArgs e ) {
			progress = e.Progress;
		}
		
		public override void Dispose() {
			font.Dispose();
			messageWidget.Dispose();
			titleWidget.Dispose();
			progressBoxTexture.Delete();
			Window.MapLoading -= MapLoading;
		}
		
		public override void OnResize( int oldWidth, int oldHeight, int width, int height ) {
			int deltaX = ( width - oldWidth ) / 2;
			messageWidget.OnResize( oldWidth, oldHeight, width, height );
			titleWidget.OnResize( oldWidth, oldHeight, width, height );
			progressBoxTexture.X1 += deltaX;
			progX += deltaX;
		}
		
		public override bool BlocksWorld {
			get { return true; }
		}
		
		public override bool HandlesAllInput {
			get { return true; }
		}
	}
}
